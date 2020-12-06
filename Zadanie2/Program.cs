using System;

namespace Zadanie2
{
    class Program
    {
                static void Main(string[] args)
        {
            var kyocera = new Copier();
            kyocera.PowerOn();
            IDocument doc1 = new PDFDocument("ez.pdf");
            kyocera.Print(in doc1);

            IDocument doc2;
            kyocera.Scan(out doc2);
            kyocera.Scan(out doc1);

            kyocera.ScanAndPrint();
            System.Console.WriteLine(kyocera.Counter);

            kyocera.PowerOff();

            kyocera.Print(in doc1);

            kyocera.Scan(out doc2);
            kyocera.Scan(out doc1);

            kyocera.ScanAndPrint();
            System.Console.WriteLine(kyocera.Counter);

            var HP = new MultidimensionalDevice();

            HP.PowerOn();

            IDocument doc3 = new PDFDocument("ez.pdf");
            HP.Print(in doc3);
            HP.Scan(out doc2);
            HP.ScanAndPrint();

            HP.Fax();
            HP.Fax(doc3);
            HP.Fax(doc2, "854963268");

            System.Console.WriteLine(HP.Counter);
        }
    }
}
