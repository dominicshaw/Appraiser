using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Appraiser.Data.Models;
using Appraiser.DTOs;
using Appraiser.Mapping;
using Appraiser.Reporting;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Appraisal = Appraiser.Data.Models.Appraisal;

namespace Appraiser.Api.Services
{
    [UsedImplicitly]
    public class ChangeService
    {
        private readonly ILogger<ChangeService> _log;
        private readonly ReportManager _reportManager;
        private readonly AppraisalMapper _appraisalMapper;

        public ChangeService(ILogger<ChangeService> log, ReportManager reportManager, AppraisalMapper appraisalMapper)
        {
            _log = log;
            _reportManager = reportManager;
            _appraisalMapper = appraisalMapper;
        }

        public async Task NotifyAppraisalChange(Appraisal appraisal, List<Change> changes)
        {
            if (!changes.Any())
                return;

            var adto = _appraisalMapper.ToDTO(appraisal, new AppraisalDTO());
            var cdtos = changes.Select(c => ChangeMapper.ToDTO(c, new ChangeDTO())).ToList();

            var path = Path.Combine(Path.GetTempPath(), Path.ChangeExtension(Path.GetTempFileName(), "html"));

            await using (var mem = _reportManager.GetAppraisalChanges(adto, cdtos))
            await using (var fil = new FileStream(path, FileMode.Create))
            {
                await mem.CopyToAsync(fil);
            }

            using var smtp = new SmtpClient("mailserver.ttint.com");
            using var message = new MailMessage { From = new MailAddress("appraiser@ttint.com") };

            if (appraisal.Staff.Email != null)
                message.To.Add(new MailAddress(appraisal.Staff.Email, appraisal.Staff.Name));
            if (appraisal.Staff.Manager != null)
                message.To.Add(new MailAddress(appraisal.Staff.Manager.Email, appraisal.Staff.Manager.Name));

            message.To.Clear(); // TODO: remove when production

            message.Bcc.Add(new MailAddress("shawd@ttint.com", "Dominic Shaw"));

            var body = await File.ReadAllTextAsync(path);

            message.Subject = $"Changes to Appraisal: {appraisal.Staff.Name}, {appraisal.PeriodStart.Year}";
            message.IsBodyHtml = true;
            message.Body = body;

            _log.LogInformation("Sending email of changes '{subject}', {count} changes.", message.Subject, changes.Count);

            smtp.Send(message);
        }
    }
}