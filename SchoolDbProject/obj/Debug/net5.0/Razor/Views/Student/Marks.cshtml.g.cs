#pragma checksum "F:\3-ий курс\SCHOOLDB COURSE PROJECT\SchoolDbProject\SchoolDbProject\Views\Student\Marks.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "615188fcf8ae06f026b2bd6b577cb72dca3011d5"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Student_Marks), @"mvc.1.0.view", @"/Views/Student/Marks.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"615188fcf8ae06f026b2bd6b577cb72dca3011d5", @"/Views/Student/Marks.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0b20b554506a3cc5403d93ad06b44441db47dcee", @"/Views/_ViewImports.cshtml")]
    public class Views_Student_Marks : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<CustomStudentMarks>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"
<style>
    .flex {
        display: flex;
    }

    .buttons div {
        padding-bottom: 10px;
        padding-top: 10px;
        padding-right: 30px;
    }

    button {
        height: 40px;
        width: 100%;
    }

    .content div {
        padding-top: 10px;
        padding-bottom: 5px;
    }
</style>

<div class=""flex"">
    <div class=""buttons"">
        <div>
            <button id=""personal"" class=""btn btn-outline-dark"">Personal</button>
        </div>
        <div>
            <button id=""schedule"" class=""btn btn-outline-dark"">Schedule</button>
        </div>
        <div>
            <button id=""marks"" class=""btn btn-outline-dark"">Marks</button>
        </div>
        <div>
            <button id=""logout"" class=""btn btn-outline-dark"">Logout</button>
        </div>
    </div>
    <div class=""content"">
");
#nullable restore
#line 41 "F:\3-ий курс\SCHOOLDB COURSE PROJECT\SchoolDbProject\SchoolDbProject\Views\Student\Marks.cshtml"
         foreach (var m in Model)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <div>\r\n                ");
#nullable restore
#line 44 "F:\3-ий курс\SCHOOLDB COURSE PROJECT\SchoolDbProject\SchoolDbProject\Views\Student\Marks.cshtml"
           Write(m.Subject);

#line default
#line hidden
#nullable disable
            WriteLiteral(":\r\n");
#nullable restore
#line 45 "F:\3-ий курс\SCHOOLDB COURSE PROJECT\SchoolDbProject\SchoolDbProject\Views\Student\Marks.cshtml"
                 foreach (var mm in m.Mark)
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <span>");
#nullable restore
#line 47 "F:\3-ий курс\SCHOOLDB COURSE PROJECT\SchoolDbProject\SchoolDbProject\Views\Student\Marks.cshtml"
                     Write(mm);

#line default
#line hidden
#nullable disable
            WriteLiteral("  </span>\r\n");
#nullable restore
#line 48 "F:\3-ий курс\SCHOOLDB COURSE PROJECT\SchoolDbProject\SchoolDbProject\Views\Student\Marks.cshtml"
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("            </div>\r\n");
#nullable restore
#line 50 "F:\3-ий курс\SCHOOLDB COURSE PROJECT\SchoolDbProject\SchoolDbProject\Views\Student\Marks.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"    </div>
</div>

<script defer type=""text/javascript"">
    personal.onclick = function () {
        window.open('https://localhost:44389/Student/Personal', '_self');
    }

    schedule.onclick = function () {
        window.open('https://localhost:44389/Student/Schedule', '_self');
    }

    marks.onclick = function () {
        window.open('https://localhost:44389/Student/Marks', '_self');
    }

    logout.onclick = function () {
        window.open('https://localhost:44389/Account/Logout', '_self');
    }
</script>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<CustomStudentMarks>> Html { get; private set; }
    }
}
#pragma warning restore 1591
