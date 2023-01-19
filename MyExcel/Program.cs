using MyExel.Spreadsheet;

namespace MyExcel
{
    internal static class Program
    {
        public static Form1 mainForm;
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            mainForm = new Form1();
            SpreadSheet.CreateSpreadSheet();
            Application.Run(mainForm);

        }
    }
}
