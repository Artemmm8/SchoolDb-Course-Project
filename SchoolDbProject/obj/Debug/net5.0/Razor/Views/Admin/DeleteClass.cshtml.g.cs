#pragma checksum "F:\3-ий курс\SCHOOLDB COURSE PROJECT\SchoolDbProject\SchoolDbProject\Views\Admin\DeleteClass.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "a07609c88ebb8c9cc21e0f5aab0c7afc3f6f8e4e"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Admin_DeleteClass), @"mvc.1.0.view", @"/Views/Admin/DeleteClass.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "F:\3-ий курс\SCHOOLDB COURSE PROJECT\SchoolDbProject\SchoolDbProject\Views\_ViewImports.cshtml"
using SchoolDbProject;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "F:\3-ий курс\SCHOOLDB COURSE PROJECT\SchoolDbProject\SchoolDbProject\Views\_ViewImports.cshtml"
using SchoolDbProject.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a07609c88ebb8c9cc21e0f5aab0c7afc3f6f8e4e", @"/Views/Admin/DeleteClass.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0b20b554506a3cc5403d93ad06b44441db47dcee", @"/Views/_ViewImports.cshtml")]
    public class Views_Admin_DeleteClass : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<CustomClass>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("liform"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "DeleteClass", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "Admin", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"
<style>
    .flex {
        display: flex;
    }

    li {
        list-style-type: none;
        padding: 10px;
        height: 50px;
    }

    * {
        margin: 0;
        padding: 0;
        box-sizing: border-box;
    }

    #li1 {
        width: 200px;
    }

    #submbtn {
        margin-left: 213px;
    }
</style>

<div>
    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "a07609c88ebb8c9cc21e0f5aab0c7afc3f6f8e4e4621", async() => {
                WriteLiteral(@"
        <div class=""flex"">
            <div>
                <ul>
                    <li>
                        Choose class:
                    </li>
                </ul>
            </div>
            <div>
                <ul>
                    <li>
                        ");
#nullable restore
#line 42 "F:\3-ий курс\SCHOOLDB COURSE PROJECT\SchoolDbProject\SchoolDbProject\Views\Admin\DeleteClass.cshtml"
                   Write(Html.DropDownListFor(c => c.SelectedClass, new SelectList(Model.Classes), " ", new { id = "li1", onchange = "getSelected1()" }));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                    </li>\r\n                </ul>\r\n            </div>\r\n            <div>\r\n                <ul>\r\n                    <li>\r\n                        <div class=\"text-danger\">");
#nullable restore
#line 49 "F:\3-ий курс\SCHOOLDB COURSE PROJECT\SchoolDbProject\SchoolDbProject\Views\Admin\DeleteClass.cshtml"
                                            Write(Html.ValidationMessageFor(c => c.SelectedClass));

#line default
#line hidden
#nullable disable
                WriteLiteral("</div>\r\n                    </li>\r\n                </ul>\r\n            </div>\r\n        </div>\r\n        <div class=\"text-danger\">\r\n            ");
#nullable restore
#line 55 "F:\3-ий курс\SCHOOLDB COURSE PROJECT\SchoolDbProject\SchoolDbProject\Views\Admin\DeleteClass.cshtml"
       Write(ViewBag.UsedInStudents);

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n            ");
#nullable restore
#line 56 "F:\3-ий курс\SCHOOLDB COURSE PROJECT\SchoolDbProject\SchoolDbProject\Views\Admin\DeleteClass.cshtml"
       Write(ViewBag.UsedInLessons);

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n        </div>\r\n        <div>\r\n            <input id=\"submbtn\" type=\"button\" value=\"Delete Class\" class=\"btn btn-outline-danger\" />\r\n        </div>\r\n    ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Controller = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n</div>\r\n\r\n<script defer type=\"text/javascript\">\r\n    submbtn.onclick = function () {\r\n        document.getElementById(\"liform\").submit();\r\n    }\r\n</script>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<CustomClass> Html { get; private set; }
    }
}
#pragma warning restore 1591
