using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    /// <summary>
    /// 在模板中可用的通用工具方法
    /// </summary>
    public class RazorHtmlHelper
    {
        private readonly TemplateBase _template;

        public RazorHtmlHelper(TemplateBase template)
        {
            this._template = template;
        }

        /// <summary>
        /// 原样输出字符串，不转义
        /// </summary>
        /// <param name="rawString"></param>
        /// <returns></returns>
        public object Raw(string rawString)
        {
            return this._template.Raw(rawString);
        }

        /// <summary>
        /// 转换相对路径
        /// </summary>
        /// <param name="templatePath"></param>
        /// <returns></returns>
        public static string ResolveTemplatePath(string templatePath)
        {
            templatePath = templatePath.Replace("/", "\\").TrimStart('\\');
            templatePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Template", templatePath);
            return templatePath;
        }

        /// <summary>
        /// 调用另一个模板生成并返回（指定路径和Model）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="templatePath"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public object Partial<T>(string templatePath, T model, bool newLine = true)
        {
            templatePath = ResolveTemplatePath(templatePath);

            CodeBuilder<T> builder = new CodeBuilder<T>(templatePath, model);
            string content = builder.Generate().Trim();
            if (newLine)
            {
                content += "\r\n";
            }
            return Raw(content);
        }

        /// <summary>
        /// 调用另一个模板生成并返回（指定路径和Model和ViewBag）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="templatePath"></param>
        /// <param name="model"></param>
        /// <param name="viewBag"></param>
        /// <param name="newLine"></param>
        /// <returns></returns>
        public object Partial<T>(string templatePath, T model, Dictionary<string, object> viewBag, bool newLine = true)
        {
            templatePath = ResolveTemplatePath(templatePath);

            CodeBuilder<T> builder = new CodeBuilder<T>(templatePath, model);
            foreach (var p in viewBag)
            {
                builder.SetViewBagValue(p.Key, p.Value);
            }
            string content = builder.Generate().Trim();
            if (newLine)
            {
                content += "\r\n";
            }
            return Raw(content);
        }
    }
}
