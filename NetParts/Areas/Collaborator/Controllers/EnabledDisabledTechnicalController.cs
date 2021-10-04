using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetParts.Libraries.Email;
using NetParts.Libraries.Filter;
using NetParts.Libraries.Lang;
using NetParts.Libraries.Login;
using NetParts.Models;
using NetParts.Models.Constant;
using NetParts.Repositories;
using NetParts.Repositories.Contracts;

namespace NetParts.Areas.Collaborator.Controllers
{
    [Area("Collaborator")]
    [CollaboratorAultorization(new String[] { CollaboratorTypeConstant.Administrador })]
    public class EnabledDisabledTechnicalController : Controller
    {
        private LoginCollaborator _loginCollaborator;
        private ITechnicalAssistanceRepository _technicalAssistanceRepository;
        private ManageEmail _manageEmail;
        private ILogger<TechnicalAssistanceController> _logger;

        public EnabledDisabledTechnicalController(ITechnicalAssistanceRepository technicalAssistanceRepository, LoginCollaborator loginCollaborator, IArchiveRepository archiveRepository, ManageEmail manageEmail, ILogger<TechnicalAssistanceController> logger)
        {
            _technicalAssistanceRepository = technicalAssistanceRepository;
            _loginCollaborator = loginCollaborator;
            _manageEmail = manageEmail;
            _logger = logger;
        }
        public IActionResult Index(int? page, string search)
        {
            var technical = _technicalAssistanceRepository.GetAllTechnicalAssistance(page, search);
            _logger.LogInformation("Listando assistências técnicas");
            return View(technical);
        }

        [ValidateHttpReferer]
        public IActionResult EnabledDisabled(int id)
        {
            TechnicalAssistance technical = _technicalAssistanceRepository.GetTechnicalAssistance(id);
            technical.EnabledDisabled = (technical.EnabledDisabled == SituationConstant.Enabled) ? technical.EnabledDisabled = SituationConstant.Disabled : technical.EnabledDisabled = SituationConstant.Enabled;
            _technicalAssistanceRepository.Update(technical);

            _manageEmail.EnabledAssistance(technical);
            TempData["MSG_S"] = Msg.MSG_S005;
            _logger.LogInformation("Ativar e desativar assistência técnica");
            return RedirectToAction(nameof(Index));
        }
    }
}