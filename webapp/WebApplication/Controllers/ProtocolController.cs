using K9.Base.WebApplication.Filters;
using K9.DataAccessLayer.Models;
using K9.SharedLibrary.Models;
using K9.WebApplication.Packages;
using K9.WebApplication.Services;
using System;
using System.Web.Mvc;

namespace K9.WebApplication.Controllers
{
    [Authorize]
    [RequirePermissions(Role = Constants.Constants.ClientUser)]
    public partial class ProtocolController : BaseRootController
    {
        private readonly IProtocolService _protocolService;
        private readonly IHealthQuestionnaireService _healthQuestionnaireService;

        public ProtocolController(IRepository<Product> productsRepository, IMembershipService membershipService, IProtocolService protocolService, IHealthQuestionnaireService healthQuestionnaireService, IServicePackage servicePackage) : base(servicePackage)
        {
            _protocolService = protocolService;
            _healthQuestionnaireService = healthQuestionnaireService;
        }

        public ActionResult Summary(Guid id)
        {
            var protocol = _protocolService.Find(id);
            protocol = _protocolService.GetProtocolWithProtocolSections(protocol.Id);
            ViewBag.ProtocolView = true;
            return View("../Protocols/Summary", protocol);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Summary(Protocol model)
        {
            var hq = _healthQuestionnaireService.GetHealthQuestionnaireForClient(model.ClientId.Value);

            hq.EatsRedMeat = model.EatsRedMeat;
            hq.EatsPoultry = model.EatsPoultry;
            hq.EatsFishAndSeafood = model.EatsFishAndSeafood;
            hq.EatsEggsAndRoes = model.EatsEggsAndRoes;
            hq.EatsDairy = model.EatsDairy;
            hq.EatsVegetables = model.EatsVegetables;
            hq.EatsVegetableProtein = model.EatsVegetableProtein;
            hq.EatsFungi = model.EatsFungi;
            hq.EatsFruit = model.EatsFruit;
            hq.EatsGrains = model.EatsGrains;
            hq.IsLowOxalate = model.IsLowOxalate;
            hq.IsLowLectin = model.IsLowLectin;
            hq.IsLowPhytate = model.IsLowPhytate;
            hq.IsLowHistamine = model.IsLowHistamine;
            hq.IsLowMycotoxin = model.IsLowMycotoxin;
            hq.IsLowOmega6 = model.IsLowOmega6;
            hq.IsBulletProof = model.IsBulletProof;
            hq.IsSattvic = model.IsSattvic;
            hq.IsLowSulphur = model.IsLowSulphur;
            hq.IsLowGoitrogen = model.IsLowGoitrogen;
            hq.IsKeto = model.IsKeto;
            hq.CurrentHealthLevel = model.CurrentHealthLevel;
            hq.AutomaticallyFilterFoods = model.AutomaticallyFilterFoods;

            if (model.AutomaticallyFilterFoods)
            {
                var threshold = hq.GetScoreThreshold();

                hq.IsLowOxalate = hq.GetOxalateScore() > threshold;
                hq.IsLowLectin = hq.GetLectinsScore() > threshold;
                hq.IsLowPhytate = hq.GetPhytateScore() > threshold;
                hq.IsLowHistamine = hq.GetHistamineScore() > threshold;
                hq.IsLowMycotoxin = hq.GetMycotoxinScore() > threshold;
                hq.IsLowOmega6 = hq.GetOmega6Score() > threshold;
                hq.IsLowSulphur = hq.GetCbsScore() > threshold;
                hq.IsKeto= hq.GetCbsScore() > threshold || hq.GetInflammationScore() > threshold;
                hq.IsLowGoitrogen = hq.HypoThyroidism;
            }

            _healthQuestionnaireService.Save(hq);

            return RedirectToAction("Summary", new { id = model.ExternalId });
        }

        public ActionResult PrintableSummary(Guid id)
        {
            var protocol = GetProtocol(id);
            return View("../Protocols/PrintableSummary", protocol);
        }

        public ActionResult PrintableSchedule(Guid id)
        {
            var protocol = GetProtocol(id);
            return View("../Protocols/PrintableSchedule", protocol);
        }

        public ActionResult PrintableGenoTypeSummary(Guid id)
        {
            var protocol = GetProtocol(id);
            return View("../Protocols/PrintableGenoTypeSummary", protocol);
        }

        public ActionResult PrintableDoshasSummary(Guid id)
        {
            var protocol = GetProtocol(id);
            return View("../Protocols/PrintableDoshasSummary", protocol);
        }

        public ActionResult PrintableNineStarKiSummary(Guid id)
        {
            var protocol = GetProtocol(id);
            return View("../Protocols/PrintableNineStarKiSummary", protocol);
        }

        public ActionResult PrintableFoodList(Guid id)
        {
            var protocol = GetProtocol(id);
            return View("../Protocols/PrintableFoodsList", protocol);
        }

        public ActionResult PrintableDietaryAdvice(Guid id)
        {
            var protocol = GetProtocol(id);
            return View("../Protocols/PrintableDietaryAdvice", protocol);
        }

        public ActionResult PrintableHealingActivities(Guid id)
        {
            var protocol = GetProtocol(id);
            return View("../Protocols/PrintableHealingActivities", protocol);
        }

        private Protocol GetProtocol(Guid id)
        {
            var protocol = _protocolService.Find(id);
            protocol = _protocolService.GetProtocolWithProtocolSections(id);
            return protocol;
        }
    }
}
