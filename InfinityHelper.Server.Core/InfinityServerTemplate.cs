using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class InfinityServerTemplate<T> : TemplateBase<T>
    {
        private readonly RazorHtmlHelper _html;
        private T _model;
        private DynamicViewBag _viewBag;

        public InfinityServerTemplate()
        {
            this._html = new RazorHtmlHelper(this);
            this.Layout = "myLayout";
        }

        /// <summary>
        /// Model/ViewBag属性需要重新定位到这一层，不然好像在cshtml中无法直接联想到TemplateBase
        /// </summary>
        /// <param name="model"></param>
        /// <param name="viewbag"></param>
        public override void SetData(object model, DynamicViewBag viewbag)
        {
            base.SetData(model, viewbag);

            this._model = (T)model;
            this._viewBag = viewbag;
        }

        /// <summary>
        /// Model
        /// </summary>
        public new T Model { get { return _model; } }

        /// <summary>
        /// ViewBag
        /// </summary>
        public new dynamic ViewBag { get { return _viewBag; } }

        /// <summary>
        /// HTML帮助类
        /// </summary>
        public RazorHtmlHelper Html { get { return _html; } }
    }
}
