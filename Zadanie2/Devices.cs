using System.Runtime.CompilerServices;
using System.Data;
using System;

namespace Zadanie2
{
    public interface IDevice
    {
        enum State {on, off};

        void PowerOn(); // uruchamia urządzenie, zmienia stan na `on`
        void PowerOff(); // wyłącza urządzenie, zmienia stan na `off
        State GetState(); // zwraca aktualny stan urządzenia

        int Counter {get;}  // zwraca liczbę charakteryzującą eksploatację urządzenia,
                            // np. liczbę uruchomień, liczbę wydrukow, liczbę skanów, ...
    }

    public abstract class BaseDevice : IDevice
    {
        protected IDevice.State state = IDevice.State.off;
        public IDevice.State GetState() => state;

        public void PowerOff()
        {
            state = IDevice.State.off;
            Console.WriteLine("... Device is off !");
        }

        public void PowerOn()
        {
            state = IDevice.State.on;
            Console.WriteLine("Device is on ...");  
        }

        public int Counter { get; private set; } = 0;
    }

    public interface IPrinter : IDevice
    {
        /// <summary>
        /// Dokument jest drukowany, jeśli urządzenie włączone. W przeciwnym przypadku nic się nie wykonuje
        /// </summary>
        /// <param name="document">obiekt typu IDocument, różny od `null`</param>
        void Print(in IDocument document);
    }
    public interface IScanner : IDevice
    {
        // dokument jest skanowany, jeśli urządzenie włączone
        // w przeciwnym przypadku nic się dzieje
        void Scan(out IDocument document, IDocument.FormatType formatType);
    }
    public interface IFax : IDevice
    {
        /// <summary>
        /// </summary>
        /// <param name="document">obiekt typu IDocument, różny od `null`</param>
        void Fax(in IDocument document = null, string telNumber = null);
    }    

    public class Printer : BaseDevice, IPrinter
    {
        public int PrintCounter {get; private set;}

        public void Print(in IDocument document)
        {
            if(this.GetState() == IDevice.State.off) return;
            this.PrintCounter++;
            System.Console.WriteLine($"{DateTime.Today} Print: {document.GetFileName()}");
        }
    }
    public class Scanner : BaseDevice, IScanner
    {
        public int ScanCounter {get; private set;}

        public void Scan(out IDocument document, IDocument.FormatType formatType = IDocument.FormatType.TXT)
        {
            document = null;
            if(this.GetState() != IDevice.State.off)
            {  
                this.ScanCounter++;
                System.Console.Write($"{DateTime.Today} Scan: ");
                string fileName = "";
                switch(formatType)
                {
                    case IDocument.FormatType.TXT:
                    fileName = $"TextScan{ScanCounter}.txt";
                    document = new TextDocument(fileName);
                    break;

                    case IDocument.FormatType.PDF:
                    fileName = $"PDFScan{ScanCounter}.pdf";
                    document = new PDFDocument(fileName);
                    break;

                    case IDocument.FormatType.JPG:
                    fileName = $"ImageScan{ScanCounter}.jpg";
                    document = new ImageDocument(fileName);
                    break;
                }
                System.Console.WriteLine(fileName);
            }
        }
    }
    public class Faxer : BaseDevice, IFax
    {
        public void Fax(in IDocument document = null, string telNumber = null)
        {
            if(this.GetState() == IDevice.State.off) return;
            if(telNumber.ToCharArray().Length != 9)
                throw new ArgumentException("Błędny numer telefonu.");
            System.Console.WriteLine($"{DateTime.Today} Fax: Wysłano do adresata o numerze {telNumber} dokument: {document.GetFileName()}.");
        }
    }


    public class Copier :BaseDevice
    {
        public Printer Printer = new Printer();
        public Scanner Scanner = new Scanner();

        new public int Counter {get; private set;}

        public void Print(in IDocument document) {Counter++; Printer.Print(in document);}
        public void Scan(out IDocument document) {Counter++; Scanner.Scan(out document);}
        public void ScanAndPrint()
        {
            IDocument document;
            Counter++;
            Scanner.Scan(out document);
            Printer.Print(in document);
        }
    }
    public class MultidimensionalDevice :BaseDevice
    {
        public Printer printer = new Printer();
        public Scanner scanner = new Scanner();
        public Faxer faxer = new Faxer();

        new public int Counter {get; private set;}

        public void Print(in IDocument document) {Counter++; printer.Print(in document);}
        public void Scan(out IDocument document) {Counter++; scanner.Scan(out document);}
        public void ScanAndPrint()
        {
            IDocument document;
            Counter++;
            scanner.Scan(out document);
            printer.Print(in document);
        }
        public void Fax(in IDocument document = null, string telNumber = null)
        {
                        IDocument documentFaxBufor;
            if(this.GetState() == IDevice.State.off) return;
            if(document == null)
            {
                System.Console.WriteLine("Skanowanie dokumentu w zasobniku...");
                Scan(out documentFaxBufor);
            }
            else
            {
                documentFaxBufor = document;
                Counter++;
            }
            if(telNumber == null)
            {
                System.Console.Write("Wprowadź numer telefonu odbiorcy: ");
                telNumber = Console.ReadLine();
            }
            Counter++;
            faxer.Fax(in document,telNumber);
        }
    }
}
