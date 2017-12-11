using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using TEBOUtils.Records;

// Notes:
// Field Position 0:
//     6 = Header for a transaction set
//     7 = Trailer for a transaction set
//     8 = Header for a block of transactions/records
//     9 = Trailer for a block of transactions/records

//      Header:
//      Field Position 4:
//			1 = Account Balance
//			2 = Car Rental Summary
//			3 = Card Account
//			4 = Cardholder
//			5 = Card Transaction
//			6 = Company
//			7 = Line Item Detail
//			8 = Line Item Summary
//			9 = Lodging Summary
//			10 = Organization
//			11 = Period
//			14 = Passenger Itinerary
//			15 = Leg-Specific Information
//			16 = Supplier
//			17 = Fleet Service
//			18 = Fleet Product
//			20 = Temporary Services
//			21 = Shipping Services
//			25 = Headquarter Relationship
//			26 = Lodging Detail
//			27 = Car Rental Detail
//			28 = Allocation
//			29 = Allocation Description
//			30 = Relationship
//			31 = Phone
//			99 = Reference Data


namespace TEBOUtils
{
    public class VcfDataFactory
    {
        #region Private

        private string[] _files;
        private int _currentFileLineNumber;

        private bool _fileIsOkay(string filePath)
        {
            bool rRet = false;

            try
            {
                if (!File.Exists(filePath))
                {
                    return false;
                }
                using (System.IO.StreamReader file = new System.IO.StreamReader(filePath))
                {
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] lineArray = TEBOUtils.Utils.LineParsers.GetSplitLine(line);

                        //  if headher for transaction set (see notes)
                        if (lineArray[0] == "6")
                        {
                            string formatType = lineArray[7].Trim();
                            if (formatType != "4.0" && formatType != "4.4")
                            {
                                //todo error handling
                                return false;
                            }
                            else
                            {
                                Debug.WriteLine("File {0} passes as {1}", filePath, formatType);
                                rRet = true;
                                break;
                            }
                        }
                    }

                    file.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return rRet;
        }

        #endregion Private

        #region Public

        public List<string> FilesWithErrors;
        public List<string> FilesToProcess;

        public List<Records.Account> AccountList;
        public List<Records.Cardholder> CardholderList;
        public List<Records.Transaction> TransactionList;

        #endregion Public

        #region Constructors

        public VcfDataFactory(string pathToFiles)
        {
            FilesWithErrors = new List<string>();
            FilesToProcess = new List<string>();

            AccountList = new List<Account>();
            CardholderList = new List<Cardholder>();
            TransactionList = new List<Transaction>();

            if (Directory.Exists(pathToFiles))
            {
                _files = Directory.GetFiles(pathToFiles);
                foreach (string t in _files)
                {
                    if (_fileIsOkay(t))
                    {
                        FilesToProcess.Add(t);
                    }
                    else
                    {
                        FilesWithErrors.Add(t);
                    }
                }
            }
            else
            {
                Console.WriteLine("{0} is not a valid file or directory.", pathToFiles);
            }


            ProcessFiles();

            JoinData();
        }

        #endregion Constructors

        #region Public

        public void JoinData()
        {
            
        }

        public void WriteDebug(string strToWrite)
        {
            Debug.WriteLine(String.Format("{0} Line {1}", strToWrite, _currentFileLineNumber));
        }
        public bool ProcessFiles()
        {
            bool rRet = false;
            try
            {
                FilesToProcess.ForEach(procFile =>
                {
                    Debug.WriteLine(String.Format("Procesing File {0}", procFile));
                    VcfFile fy = new VcfFile(procFile);
                    _currentFileLineNumber = 0;
                    while (_currentFileLineNumber < fy.RecordLines.Count)
                    {
                        string[] recLine = fy.RecordLines[_currentFileLineNumber];

                        //start of records
                        if (recLine[0] == "6")
                        {
                            WriteDebug("Start of Records");



                        }
                        if (recLine[0] == "7")
                        {
                            WriteDebug("End of Records");
                        }
                        if (recLine[0] == "8")
                        {
                            WriteDebug("Enter Block");

                            switch (recLine[4])
                            {
                                //cardholder
                                case "04":
                                    _currentFileLineNumber++;
                                    CardholderList.AddRange(ProcessCardHolders(ref fy.RecordLines));
                                    break;
                                //account
                                case "03":
                                    _currentFileLineNumber++;
                                    AccountList.AddRange(ProcessAccounts(ref fy.RecordLines));
                                    break;

                                //transaction
                                case "05":
                                    _currentFileLineNumber++;
                                    TransactionList.AddRange(ProcessTransaction(ref fy.RecordLines));
                                    break;
                                default:
                                    WriteDebug("Nothing");
                                    break;
                            }

                        }
                        _currentFileLineNumber++;
                    }

                });

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return rRet;
        }

        private List<Transaction> ProcessTransaction(ref List<string[]> recordLines)
        {

            List<Transaction> rRet = new List<Transaction>();
            while (_currentFileLineNumber < recordLines.Count)
            {
                string[] trimLines = Utils.LineParsers.TrimLineArray(recordLines[_currentFileLineNumber]);

                if (trimLines[0] == "9")
                {
                    WriteDebug("Done With Transaction Block");
                    return rRet;
                }
                Transaction toAdd = new Transaction
                {
                    TransactionReferenceNumber = trimLines[3],
                    AccountNumber = trimLines[1],
                    TransactionType = GetTransactionTypeCode(trimLines[17]),
                    //SourceAmount = ,
                    SourceCurrencyCode = trimLines[15],
                    //BillingAmount = ,
                    //TaxAmount = ,
                    CustomerVatNumber = trimLines[27],
                    MemoPostFlag = trimLines[51],
                    CardAcceptor = trimLines[7],
                    SupplierName = trimLines[8],
                    SupplierCity = trimLines[9],
                    SupplierState = trimLines[10],
                    SupplierPostalCode = trimLines[12],
                    SupplierISOCountryCode = trimLines[11],
                    SupplierVatNumber = trimLines[25]


                };

                if (DateTime.TryParseExact(trimLines[2], "MMddyyyy", new CultureInfo("en-US"), DateTimeStyles.None, out var postDate))
                {
                    toAdd.PostingDate = postDate;
                }
                else
                {
                    //todo log date parse error
                    System.Diagnostics.Debugger.Break();
                }

                if (Decimal.TryParse(trimLines[13], out var sourceAmount))
                {
                    toAdd.SourceAmount = sourceAmount;
                }
                else
                {
                    //todo log decimal parse error
                    System.Diagnostics.Debugger.Break();
                }

                if (Decimal.TryParse(trimLines[20], out var taxAmount))
                {
                    toAdd.TaxAmount = taxAmount;
                }
                else
                {
                    //todo log decimal parse error
                    System.Diagnostics.Debugger.Break();
                }

                rRet.Add(toAdd);
                string outPut = string.Join(",", trimLines);
                WriteDebug(outPut);
                _currentFileLineNumber++;
            }
            throw new Exception("Error in Transaction");

        }



        private TransactionTypes GetTransactionTypeCode(string trimLine)
        {
            try
            {
                if (Int32.TryParse(trimLine, out int tTypeValue))
                {
                    TransactionTypes rRet = (TransactionTypes)tTypeValue;
                    return rRet;
                }
                else
                {
                    //todo log enum convert error
                    System.Diagnostics.Debugger.Break();
                    throw new Exception("Error in Transaction Type Conversion");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        private List<Cardholder> ProcessCardHolders(ref List<string[]> recordLines)
        {
            List<Cardholder> rRet = new List<Cardholder>();
            while (_currentFileLineNumber < recordLines.Count)
            {

                try
                {
                    string[] trimLines = Utils.LineParsers.TrimLineArray(recordLines[_currentFileLineNumber]);


                    if (trimLines[0] == "9")
                    {
                        WriteDebug("Done With Cardholder Block");
                        return rRet;
                    }
                    Cardholder toAdd = new Cardholder
                    {
                        AddressLine1 = trimLines[6],
                        AddressLine2 = trimLines[7],
                        CardHolderId = trimLines[2],
                        City = trimLines[8],
                        EmployeeId = trimLines[22],
                        FirstName = trimLines[4],
                        LastName = trimLines[5],
                        PostalCode = trimLines[11],
                        State = trimLines[11]
                    };

                    rRet.Add(toAdd);
                    string outPut = string.Join(",", trimLines);
                    WriteDebug(outPut);
                    _currentFileLineNumber++;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }



            throw new Exception("Error in Cardholders");
        }


        public List<Account> ProcessAccounts(ref List<string[]> recordList)
        {
            List<Account> rRet = new List<Account>();
            while (_currentFileLineNumber < recordList.Count)
            {
                try
                {
                    string[] trimLines = Utils.LineParsers.TrimLineArray(recordList[_currentFileLineNumber]);


                    if (trimLines[0] == "9")
                    {
                        WriteDebug("Done With Accounts Block");
                        return rRet;
                    }

                    Account toAdd = new Account
                    {
                        AccountNumber = trimLines[2],
                        CardHolderId = trimLines[1]
                    };
                    if (DateTime.TryParseExact(trimLines[4], "MMddyyyy", new CultureInfo("en-US"), DateTimeStyles.None, out var efDate))
                    {
                        toAdd.EffectiveDate = efDate;
                    }
                    else
                    {
                        //todo log date parse error
                        System.Diagnostics.Debugger.Break();
                    }

                    if (DateTime.TryParseExact(trimLines[7], "MMddyyyy", new CultureInfo("en-US"), DateTimeStyles.None, out var exDate))
                    {
                        toAdd.ExpireDate = exDate;
                    }
                    else
                    {
                        //todo log date parse error
                        System.Diagnostics.Debugger.Break();
                    }
                    rRet.Add(toAdd);

                    string outPut = string.Join(",", trimLines);
                    WriteDebug(outPut);
                    _currentFileLineNumber++;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }


            throw new Exception("Error in Accounts");
        }
        #endregion Public
    }
}