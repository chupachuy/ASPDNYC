using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DNyC.Models
{
    /// <summary>
    /// Clase para enviar parámetros al componente de FileUpload
    /// </summary>
    public class FileUploadViewModel
    {
        public FileUploadViewModel()
        {
        }

        public FileUploadViewModel(string elementId,  string parametersFunction = null)
        {
            this.ElementId = elementId;
            this.ParametersFunction = parametersFunction;
        }


        public FileUploadViewModel(string elementId, string boxText, string TitleText, string TitleClass, string parametersFunction = null)
        {
            this.ElementId = elementId;
            this.BoxText = boxText;
            this.TitleText = TitleText;
            this.TitleClass = TitleClass;
            this.ParametersFunction = parametersFunction;
        }

        public string ElementId { get; set; } = "fileUploadId" + Guid.NewGuid();
        public string BoxText { get; set; } = "Sube Archivo";
        public string TitleText { get; set; } = "Sube Archivo";
        public string TitleClass { get; set; } = "title_doc";
        public string ParametersFunction { get; set; }
    }
}