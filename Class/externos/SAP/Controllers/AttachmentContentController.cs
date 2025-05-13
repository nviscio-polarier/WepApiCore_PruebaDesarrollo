using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace WebApiCore.Class.externos.SAP.Controllers
{
    public class AttachmentContentController : ODataController
    {
        public async Task<HttpResponseMessage> Get(string DocumentInfoRecordDocType, string DocumentInfoRecordDocNumber, string DocumentInfoRecordDocPart, string DocumentInfoRecordDocVersion, string LogicalDocument, string ArchiveDocumentID, string LinkedSAPObjectKey, string BusinessObjectTypeName = "BKPF")
        {
            string endpoint = $"content?" +
                $"DocumentInfoRecordDocType={DocumentInfoRecordDocType}" +
                $"&DocumentInfoRecordDocNumber={DocumentInfoRecordDocNumber}" +
                $"&DocumentInfoRecordDocPart={DocumentInfoRecordDocPart}" +
                $"&DocumentInfoRecordDocVersion={DocumentInfoRecordDocVersion}" +
                $"&LogicalDocument={LogicalDocument}" +
                $"&ArchiveDocumentID={ArchiveDocumentID}" +
                $"&LinkedSAPObjectKey={LinkedSAPObjectKey}" +
                $"&BusinessObjectTypeName={BusinessObjectTypeName}";
            return await SAPHttpService.Get_Attachments(endpoint);
        }
    }
}
