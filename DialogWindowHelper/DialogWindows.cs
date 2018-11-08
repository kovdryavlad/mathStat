using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DialogWindowHelper
{
    public class DialogWindows
    {

        //диалог открытия файла
        /// <summary>
        /// Открывает диалоговое окно сохранения файла
        /// </summary>
        /// <returns>Возвращает какая кнопка была нажата на диалоговом окне. OK - true, Cancel - false</returns>
        public static bool GetOpenFileName(out string FileName)
        {
            //создание окна
            OpenFileDialog ofd = new OpenFileDialog();

            //вызов окна
            return ShowDialogWindow(ofd, out FileName);
        }
        /// <summary>
        /// Открывает диалоговое окно открытия файла
        /// </summary>
        /// <param name="FileName">out переменная для записи пути к файлу</param>
        /// <param name="filter">Фильтр для диалогового окна. Пример для передачи: 'Все файлы|*.*'</param>
        /// <returns>Возвращает какая кнопка была нажата на диалоговом окне. OK - true, Cancel - false</returns>
        public static bool GetOpenFileName(out string FileName, string filter)
        {
            //создаем окно
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;

            //добавляем фильтр
            ofd.Filter = filter;
            //вызов окна
            return ShowDialogWindow(ofd, out FileName);
        }

        //диалог сохранения файла
        /// <summary>
        /// Открывает диалоговое окно сохранения файла
        /// </summary>
        /// <param name="FileName">out переменная для записи пути к файлу</param>
        /// <returns>Возвращает какая кнопка была нажата на диалоговом окне. OK - true, Cancel - false</returns>
        public static bool GetSaveFileName(out string FileName)
        {
            //создаем окно
            SaveFileDialog sfl = new SaveFileDialog();

            //вызов окна
            return ShowDialogWindow(sfl, out FileName);
        }
        /// <summary>
        /// Открывает диалоговое окно сохранения файла
        /// </summary>
        /// <param name="FileName">out переменная для записи пути к файлу</param>
        /// <param name="extension">Расширение, которое будет дописываться, если пользователь не указал. Пример для передачи: '.xml'</param>
        /// <returns>Возвращает какая кнопка была нажата на диалоговом окне. OK - true, Cancel - false</returns>
        public static bool GetSaveFileName(out string FileName, string extension)
        {
            //создаем окно
            SaveFileDialog sfl = new SaveFileDialog();

            //добавляем расширение
            sfl.AddExtension = true;
            sfl.DefaultExt = extension;

            //вызов окна
            return ShowDialogWindow(sfl, out FileName);
        }

        //общий метод для диалоговых окон openFile и SaveFile
        private static bool ShowDialogWindow(FileDialog dialogWindow, out string FileName)
        {
            if (dialogWindow.ShowDialog() == DialogResult.OK)
            {
                FileName = dialogWindow.FileName;
                return true;
            }
            else
            {
                FileName = "";
                return false;
            }
        }

    }
}
