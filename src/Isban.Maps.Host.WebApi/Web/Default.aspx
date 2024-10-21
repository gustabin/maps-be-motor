<%@ page language="C#" autoeventwireup="true" codebehind="Default.aspx.cs" inherits="Isban.MapsMB.Host.Webapi.Default" %>


    <div>
        <ul>
            <li><%= System.DateTime.Now.ToString() %></li>
            <li><%Response.Write("Current Culture is " + System.Globalization.CultureInfo.CurrentCulture.Name); %></li>
        </ul>
        <%Isban.Mercados.DataAccess.OracleClient.HelperDataAccess.ClearCache(); %>
        <%Isban.Mercados.WebApiClient.WebApiUriHelper.Instance.Clear(); %>
        <%Oracle.ManagedDataAccess.Client.OracleConnection.ClearAllPools();%>
    </div>

