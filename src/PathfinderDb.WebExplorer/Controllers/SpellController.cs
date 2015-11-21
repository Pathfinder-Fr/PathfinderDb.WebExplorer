namespace DbBrowser.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using DbBrowser.Models;
    using PathfinderDb.Schema;

    public class SpellController : Controller
    {
        public ActionResult Index(SpellIndexQuery query)
        {
            var model = new SpellIndexViewModel();


            Func<Spell, bool> predicate = _ => true;

            predicate = predicate.AndAlso(query.CreateSourcePredicate());
            predicate = predicate.AndAlso(query.CreateListPredicate());

            model.Spells = this.DataSet().Spells.Where(predicate).OrderBy(s => s.Name).ToList();

            if (string.IsNullOrEmpty(query.List))
            {
                query.ViewMode = SpellIndexViewMode.Alphabetical;
            }
            
            if (!string.IsNullOrEmpty(query.List) && query.ViewMode == SpellIndexViewMode.ByLevel)
            {
                model.SpellGroups = model.Spells
                    .GroupBy(sg => sg.Levels.First(l => l.List == query.List).Level)
                    .OrderBy(g => g.Key);
            }

            model.Query = query;
            model.AllSources = SourceExtensions.AllSources;

            return this.View(model);
        }

        private string AsLevelGroup(int level)
        {
            switch (level)
            {
                case 0: return "Oraison/Tour";
                case 1: return "Niveau 1";
                case 2: return "Niveau 2";
                case 3: return "Niveau 3";
                case 4: return "Niveau 4";
                case 5: return "Niveau 5";
                case 6: return "Niveau 6";
                case 7: return "Niveau 7";
                case 8: return "Niveau 8";
                case 9: return "Niveau 9";
                default: return "Niveau ??";
            }
        }
    }
}