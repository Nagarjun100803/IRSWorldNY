<%@ Page Language="C#" MasterPageFile="Default.master" AutoEventWireup="true"
    Inherits="IRSWorld.Admin.PersonalizationPage" Title="Untitled Page" Codebehind="PersonalizationPage.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="bharosaPage">
        <script type="text/javascript">
            function goto_main()
            {
                top.load_center();
            }
            
            function validateFields() {
                return true;
            }            
        </script>
        <div class="bharosaPageBody">
            <div class="bharosaPageTitle">
                Your New Security Profile
            </div>
            <div class="bharosaPageMessage">
                Setting up your new security profile enhances your online protection.<br />
                It adds new layers of security to your account by helping us identify you - and
                will help you identify our site.
            </div>
            <div class="registerSection">
                <div class="sectionImage">
                    <img src="../bharosa_web/images/UIO_TP_sample.jpg" alt="Text Pad sample"/></div>
                <div class="sectionTitle">
                    Step 1: Security Image and Phrase</div>
                <div class="sectionSubTitle">
                    Enhanced data security</div>
                <div class="sectionBody">
                    Your new personalized security devices will help protect you while banking online.
                    Information you enter is protected from many of the security threats out there today.
                    At the same time the image, phrase and date are proof that you are on our official
                    site.</div>
            </div>
            <div class="registerSection">
                <div class="sectionImage">
                    <img src="../bharosa_web/images/UIO_QP_sample.jpg" alt="Question Pad sample"/></div>
                <div class="sectionTitle">
                    Step 2: Security Questions and Answers</div>
                <div class="sectionSubTitle">
                    Additional layer of security</div>
                <div class="sectionBody">
                    You will register three security questions to add another layer of security. In
                    the future we will ask you one of these questions using your personalized device
                    if a situation seems risky. These questions and answers should be kept secret just
                    like your password.</div>
            </div>
            <table cellpadding=0 cellspacing=0 style="width:100%">
                <tr>
                    <td style="width:50%" align=left>
                        <asp:PlaceHolder ID="skipPlaceholder" runat="server" Visible="false">
                            <div class="registerSubmit">
                                To skip registration at this time click >>
                                <asp:Button ID="btnSkip" CssClass="submitButton" runat="server" Text="Skip" OnClick="btnSkip_Click" />
                            </div> 
                        </asp:PlaceHolder>                   
                    </td>
                    <td style="width:50%" align=right>
                        <div class="registerSubmit">
                            To register your security profile now >>
                            <asp:Button ID="btnSubmit" CssClass="submitButton" runat="server" Text="Continue"
                                OnClick="btnSubmit_Click" />
                        </div>                    
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
