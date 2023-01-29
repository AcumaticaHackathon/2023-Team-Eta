<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="ACPR1000.aspx.cs" Inherits="Page_ACPR1000" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" TypeName="ACPROpenAI.Graph.ACPRSetupMaint" PrimaryView="Setup">
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="Setup" Width="100%" AllowAutoHide="false">
        <Template>
            <px:PXLayoutRule ID="PXLayoutRule1" runat="server" StartRow="True"></px:PXLayoutRule>
            <px:PXLayoutRule runat="server" ID="CstPXLayoutRule1" StartColumn="True"></px:PXLayoutRule>
            <px:PXLayoutRule ControlSize="XM" LabelsWidth="XM" GroupCaption="General" runat="server" ID="CstPXLayoutRule3" StartGroup="True"></px:PXLayoutRule>
            <px:PXDropDown runat="server" ID="CstPXDropDown13" DataField="Model"></px:PXDropDown>
            <px:PXNumberEdit runat="server" ID="CstPXNumberEdit8" DataField="Temperature"></px:PXNumberEdit>
            <px:PXNumberEdit runat="server" ID="CstPXNumberEdit9" DataField="MaxTokens"></px:PXNumberEdit>
            <px:PXNumberEdit runat="server" ID="CstPXNumberEdit10" DataField="TopP"></px:PXNumberEdit>
            <px:PXNumberEdit runat="server" ID="CstPXNumberEdit11" DataField="FrequencyPenalty"></px:PXNumberEdit>
            <px:PXNumberEdit runat="server" ID="CstPXNumberEdit12" DataField="PresencePenalty"></px:PXNumberEdit>
            <px:PXLayoutRule runat="server" ID="CstPXLayoutRule2" StartColumn="True"></px:PXLayoutRule>
            <px:PXLayoutRule ControlSize="XM" LabelsWidth="XM" GroupCaption="Connection" runat="server" ID="CstPXLayoutRule4" StartGroup="True"></px:PXLayoutRule>
            <px:PXTextEdit runat="server" ID="CstPXTextEdit5" DataField="APIKey"></px:PXTextEdit>
            <px:PXTextEdit runat="server" ID="CstPXTextEdit6" DataField="EndPoint"></px:PXTextEdit>
        </Template>
        <AutoSize Container="Window" Enabled="True" MinHeight="200"></AutoSize>
    </px:PXFormView>
    <px:PXSmartPanel Caption="Open AI Connection Test" CaptionVisible="True" Key="TestView" LoadOnDemand="True" Width="700" AllowResize="False" AutoReload="True" AutoRepaint="True" Position="CenterWindow" ShowAfterLoad="True" runat="server" ID="CstTestViewSmartPanel">
        <px:PXFormView DataSourceID="ds" Width="100%" SkinID="Transparent" DataMember="TestView" runat="server" ID="CstTestViewForm">
            <Template>
                <px:PXLayoutRule runat="server" ID="CstPXLayoutRule18" StartRow="True"></px:PXLayoutRule>
                <px:PXLayoutRule runat="server" ID="CstPXLayoutRule19" StartColumn="True"></px:PXLayoutRule>
                <px:PXLayoutRule ControlSize="XXL" LabelsWidth="XXL" GroupCaption="Test Q/A" runat="server" ID="CstPXLayoutRule20" StartGroup="True"></px:PXLayoutRule>
                <px:PXTextEdit Size="" TextMode="MultiLine" Width="100%" Height="250" SuppressLabel="True" TextAlign="Left" runat="server" ID="CstPXTextEdit21" DataField="Answer"></px:PXTextEdit>
                <px:PXLayoutRule runat="server" ID="CstPXLayoutRule22" StartRow="True"></px:PXLayoutRule>
                <px:PXLayoutRule ControlSize="XXL" LabelsWidth="XXL" runat="server" ID="CstPXLayoutRule23" StartColumn="True"></px:PXLayoutRule>
                <px:PXLayoutRule runat="server" ID="CstPXLayoutRule25" Merge="True"></px:PXLayoutRule>
                <px:PXTextEdit Placeholder="" SuppressLabel="False" Width="100%" TextMode="SingleLine" runat="server" ID="CstPXTextEdit24" DataField="Prompt"></px:PXTextEdit>
                <px:PXButton CommandSourceID="ds" CommandName="sendRequest" Text="Send" runat="server" ID="CstButton26"></px:PXButton>
            </Template>
        </px:PXFormView>
        <px:PXPanel SkinID="Buttons" runat="server" ID="CstPanel16">
            <px:PXButton DialogResult="Cancel" Text="Ok" runat="server" ID="CstButton17"></px:PXButton>
        </px:PXPanel>
    </px:PXSmartPanel>
</asp:Content>
