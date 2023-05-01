using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHruParser
{
    public class VacancyCategories
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public List<Vacancy> Vacancies { get; set; }
    }
}
