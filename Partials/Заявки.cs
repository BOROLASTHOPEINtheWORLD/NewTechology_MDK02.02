using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTechn
{
    public partial class ЗаявкиПартнеров
    {
        public string Title => $"{Партнеры.ТипыПартнеров.Название} | {Партнеры.Наименование}";
        public string Address => $"{Партнеры.ЮридическийАдрес}";
        public string Phone => $"+7 {Партнеры.Телефон}";
        public string Rating => $"Рейтинг: {Партнеры.Рейтинг}";
        public string Cost => $"Стоимость: {Total:F2} р";
        public decimal Total => СоставЗаявки?.Sum(s=> s.КоличествоПродукции* s.Продукция?.МинимальнаяСтоимостьДляПартнера) ?? 0;

        public string ListProd
        {
            get
            {
                if(СоставЗаявки != null)
                {
                    var products = СоставЗаявки?.Select(z => $"{z.Продукция?.НаименованиеПродукции} - {z.КоличествоПродукции} шт");
                    return string.Join("\n", products );
                }
                return string.Empty;
            }
        }
     
    }
}
