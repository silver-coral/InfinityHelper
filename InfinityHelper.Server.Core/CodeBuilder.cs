using RazorEngine.Configuration;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    /// <summary>
    /// 代码生成工具
    /// </summary>
    public class CodeBuilder<T>
    {
        /// <summary>
        /// 当前使用的Template名称
        /// </summary>
        public virtual string TemplateName { get { return this._templateName; } }

        /// <summary>
        /// 当前使用的模板文件路径
        /// </summary>
        public virtual string TemplatePath { get { return this._templatePath; } }

        /// <summary>
        /// 当前Template使用的Model类型
        /// </summary>
        public Type ModelType { get { return typeof(T); } }

        /// <summary>
        /// ViewBag
        /// </summary>
        public dynamic ViewBag { get { return _viewBag; } }

        /// <summary>
        /// Razor引擎
        /// </summary>
        private static readonly IRazorEngineService _razor;

        /// <summary>
        /// 
        /// </summary>
        private readonly DynamicViewBag _viewBag;
        private readonly T _model;
        private readonly string _templatePath;
        private readonly string _templateName;

        static CodeBuilder()
        {
            //初始化Razor
            var config = new TemplateServiceConfiguration();
            config.BaseTemplateType = typeof(InfinityServerTemplate<>);
            _razor = RazorEngineService.Create(config);
        }

        public CodeBuilder(string templatePath, T model)
            : this(model)
        {
            this._templatePath = templatePath;
            this._templateName = Path.GetFileNameWithoutExtension(templatePath);
        }

        protected CodeBuilder(T model)
        {
            this._model = model;
            this._viewBag = new DynamicViewBag();
        }

        /// <summary>
        /// 使用key-value方式给ViewBag赋值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public virtual void SetViewBagValue(string key, object value)
        {
            _viewBag.AddValue(key, value);
        }

        /// <summary>
        /// 有时候需要利用统一的布局/母版
        /// </summary>
        public virtual void LoadLayout(string layoutTemplate)
        {
            _razor.AddTemplate("myLayout", File.ReadAllText(layoutTemplate));
        }

        /// <summary>
        /// 从给定model生成代码字符串
        /// TODO:里面对模板进行了缓存，为了支持模板文件修改后自动清除缓存，需要实现一个文件内容监视功能
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Generate()
        {
            ITemplateKey key = _razor.GetKey(this.TemplatePath);

            if (!_razor.IsTemplateCached(key, this.ModelType)) //第一次没缓存
            {
                string template = File.ReadAllText(this.TemplatePath); //读取模板文件内容
                _razor.AddTemplate(key, template); //添加模板
            }

            try
            {
                //输出
                using (TextWriter tw = new StringWriter())
                {
                    _razor.RunCompile(key, tw, this.ModelType, this._model, (DynamicViewBag)this.ViewBag); //编译模板得到结果，并缓存
                    return tw.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}，模板运行出错", this.TemplatePath), ex);
            }
        }
    }
}
