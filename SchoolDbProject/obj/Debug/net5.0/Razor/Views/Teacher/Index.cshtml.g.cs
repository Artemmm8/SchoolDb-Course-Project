#pragma checksum "F:\3-ий курс\SCHOOLDB COURSE PROJECT\SchoolDbProject\SchoolDbProject\Views\Teacher\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "d68539909a0116c0daaea6033341b35dde81e7d2"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Teacher_Index), @"mvc.1.0.view", @"/Views/Teacher/Index.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d68539909a0116c0daaea6033341b35dde81e7d2", @"/Views/Teacher/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0b20b554506a3cc5403d93ad06b44441db47dcee", @"/Views/_ViewImports.cshtml")]
    public class Views_Teacher_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"<style>
    .center {
        position: absolute;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%);
        padding: 20px 20px;
    }

    #log {
        width: 150px;
        height: 40px;
    }

    #reg {
        width: 150px;
        height: 40px;
    }

    .choose {
        text-align: center;
        padding: 10px 10px;
    }
</style>

<div class=""center"">
    <div>
        <button id=""log"" class=""btn btn-outline-dark"">Login</button>
        <button id=""reg"" class=""btn btn-outline-dark"">Register</button>
    </div>

</div>
<script defer type=""text/javascript"">
    log.onclick = function () {
        window.open('https://localhost:44389/Account/TeacherLogin', '_self');
    }

    reg.onclick = function () {
        window.open('https://localhost:44389/Account/TeacherRegister', '_self');
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
