using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NetParts.Libraries.File;
using NetParts.Libraries.Filter;
using NetParts.Libraries.Lang;
using NetParts.Libraries.Login;
using NetParts.Models;
using NetParts.Models.Constant;
using NetParts.Models.ViewModels;
using NetParts.Repositories.Contracts;

namespace NetParts.Areas.Collaborator.Controllers
{
    [Area("Collaborator")]
    [CollaboratorAultorization(new String[] { CollaboratorTypeConstant.Gerente, CollaboratorTypeConstant.Administrador, CollaboratorTypeConstant.Comum })]
    public class TechnicalAssistanceController : Controller
    {
        private LoginCollaborator _loginCollaborator;
        private ITechnicalAssistanceRepository _technicalAssistanceRepository;
        private static IConfiguration _configuration;
        private IArchiveRepository _archiveRepository;
        private ILogger<TechnicalAssistanceController> _logger;

        public TechnicalAssistanceController(ITechnicalAssistanceRepository technicalAssistanceRepository, LoginCollaborator loginCollaborator, IArchiveRepository archiveRepository, IConfiguration configuration, ILogger<TechnicalAssistanceController> logger)
        {
            _technicalAssistanceRepository = technicalAssistanceRepository;
            _loginCollaborator = loginCollaborator;
            _archiveRepository = archiveRepository;
            _configuration = configuration;
            _logger = logger;
        }
        public IActionResult Index(int? page)
        {
            var loginUsuario = _loginCollaborator.GetCollaborator();
            var technical = _technicalAssistanceRepository.GetAllTechnicalAssistance(page, loginUsuario.IdTecAssistance);
            _logger.LogInformation("Listando assistência técnica");
            return View(technical);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            TechnicalAssistance technical = _technicalAssistanceRepository.GetTechnicalAssistance(id);
            _logger.LogInformation("Buscando assistência técnica pelo id");
            return View(technical);
        }

        [HttpPost]
        public IActionResult Update(TechnicalAssistance technical, int id)
        {
            if (ModelState.IsValid)
            {
                _technicalAssistanceRepository.Update(technical);

                List<Archive> ListArchivesDef = ManagerFile.MoveArchivesAssistance(new List<string>(Request.Form["archive"]), technical.IdTecAssistance.Value);

                _archiveRepository.DeleteArchivesAssistance(technical.IdTecAssistance.Value);
                _archiveRepository.CreateArchives(ListArchivesDef, technical.IdTecAssistance.Value);

                TempData["MSG_S"] = Msg.MSG_S001;
                _logger.LogInformation("Atualizando informações da assistência técnica");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                technical.Archives = new List<string>(Request.Form["archive"]).Where(a => a.Trim().Length > 0).Select(a => new Archive() { Way = a }).ToList();
                _logger.LogError("Erro ao atualizar informações da assistência técnica");
                return View(technical);
            }
        }
        [HttpGet]
        [ValidateHttpReferer]
        public IActionResult Delete(int id)
        {
            TechnicalAssistance technical = _technicalAssistanceRepository.GetTechnicalAssistance(id);
            ManagerFile.DeleteArchivesAssistance(technical.Archives.ToList());
            _archiveRepository.DeleteArchivesAssistance(id);
            _archiveRepository.Delete(id);

            TempData["MSG_S"] = Msg.MSG_S002;
            _logger.LogInformation("Assistência técnica excluída");
            return RedirectToAction(nameof(Index));
        }
        [ValidateHttpReferer]
        public IActionResult EnabledDisabled(int id)
        {
            TechnicalAssistance technical = _technicalAssistanceRepository.GetTechnicalAssistance(id);
            technical.EnabledDisabled = (technical.EnabledDisabled == SituationConstant.Enabled) ? technical.EnabledDisabled = SituationConstant.Disabled : technical.EnabledDisabled = SituationConstant.Enabled;
            _technicalAssistanceRepository.Update(technical);

            TempData["MSG_S"] = Msg.MSG_S001;
            _logger.LogInformation("Desativar assistência técnica");
            return RedirectToAction(nameof(Index));
        }
    }
}