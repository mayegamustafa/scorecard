<%@ Page Language="C#" AutoEventWireup="true" CodeFile="home.aspx.cs" Inherits="home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <title>School Score Card</title>
    <style type="text/css">
        body {
            font-family: Arial, sans-serif;
            padding: 20px;
        }
        #form1 {
            max-width: 800px;
            margin: auto;
        }
        h1 {
            text-align: center;
        }
        .input-group {
            margin-bottom: 15px;
        }
        .input-group label {
            display: inline-block;
            width: 100px;
        }
        .input-group input, .input-group select {
            width: 200px;
        }
        .gridview-container {
            margin-top: 20px;
        }
        .gridview-container h2 {
            text-align: center;
            margin-bottom: 10px;
        }

        /* Additional CSS for further customization */
    .gridview-header {
        background-color: #333333;
        color: white;
        font-weight: bold;
        text-align: center;
    }
    .gridview-row {
        background-color: #f2f2f2;
    }
    .gridview-alternating-row {
        background-color: white;
    }
    .gridview-cell {
        text-align: center;
    }

    </style>

    <script>
        // Additional JavaScript for interactivity
        document.addEventListener('DOMContentLoaded', function () {
            var gridview = document.getElementById('<%= GridView6.ClientID %>');
        // Example: Add click event to rows
        Array.from(gridview.getElementsByTagName('tr')).forEach(function (row) {
            row.addEventListener('click', function () {
                alert('Row clicked: ' + row.rowIndex);
            });
        });
    });
</script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblUsername" runat="server" Text="Label"></asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lblDepartment" runat="server" Text="Label"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp; SCORE CARD<br />
        <br />
    
    </div>
        school&nbsp;
        <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="score" DataTextField="school_name" DataValueField="school_id">
        </asp:DropDownList>
        <asp:SqlDataSource ID="score" runat="server" ConnectionString="<%$ ConnectionStrings:s_cardConnectionString %>" SelectCommand="SELECT * FROM [schools]"></asp:SqlDataSource>
        <br />
        area&nbsp;&nbsp; <asp:DropDownList ID="DropDownList2" runat="server" DataSourceID="area" DataTextField="area_name" DataValueField="area_id">
        </asp:DropDownList>
        <asp:SqlDataSource ID="area" runat="server" ConnectionString="<%$ ConnectionStrings:s_cardConnectionString %>" SelectCommand="SELECT * FROM [areas]"></asp:SqlDataSource>
        <br />
        week <asp:DropDownList ID="DropDownList3" runat="server" DataSourceID="wk" DataTextField="week_name" DataValueField="week_id">
        </asp:DropDownList>
        <asp:SqlDataSource ID="wk" runat="server" ConnectionString="<%$ ConnectionStrings:s_cardConnectionString %>" SelectCommand="SELECT * FROM [weeks]"></asp:SqlDataSource>
        <br />
        score&nbsp;&nbsp;
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <br />
        <br />
        &nbsp;&nbsp; <br />
        <br />
        <asp:Button ID="Button1" runat="server" Text="save" OnClick="Button1_Click" />
        <br />
        <br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; scores<br />
        <asp:GridView ID="GridView4" runat="server" AutoGenerateColumns="False" DataSourceID="sel" EnableModelValidation="True">
            <Columns>
                <asp:BoundField DataField="area_name" HeaderText="area_name" SortExpression="area_name" />
                <asp:BoundField DataField="school_name" HeaderText="school_name" SortExpression="school_name" />
                <asp:BoundField DataField="week_name" HeaderText="week_name" SortExpression="week_name" />
                <asp:BoundField DataField="score" HeaderText="score" SortExpression="score" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="sel" runat="server" ConnectionString="<%$ ConnectionStrings:s_cardConnectionString %>" SelectCommand="SELECT * FROM [finance_analysis]"></asp:SqlDataSource>
        <asp:SqlDataSource ID="ss" runat="server" ConnectionString="<%$ ConnectionStrings:s_cardConnectionString %>" SelectCommand="SELECT * FROM [scores]"></asp:SqlDataSource>

        <br />
        <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; all
        <br />
        <br />
        <asp:GridView ID="GridView5" runat="server" AutoGenerateColumns="False" DataKeyNames="score_id" DataSourceID="ss" EnableModelValidation="True">
            <Columns>
                <asp:BoundField DataField="score_id" HeaderText="score_id" InsertVisible="False" ReadOnly="True" SortExpression="score_id" />
                <asp:BoundField DataField="department_id" HeaderText="department_id" SortExpression="department_id" />
                <asp:BoundField DataField="school_id" HeaderText="school_id" SortExpression="school_id" />
                <asp:BoundField DataField="area_id" HeaderText="area_id" SortExpression="area_id" />
                <asp:BoundField DataField="week_id" HeaderText="week_id" SortExpression="week_id" />
                <asp:BoundField DataField="score" HeaderText="score" SortExpression="score" />
            </Columns>
        </asp:GridView>
        <br />
        finance<br />
        <br />
        
        <asp:GridView ID="GridView6" runat="server" AutoGenerateColumns="False" DataSourceID="piv" EnableModelValidation="True" 
              HeaderStyle-BackColor="#333333" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="True"
              RowStyle-BackColor="#f2f2f2" AlternatingRowStyle-BackColor="White" CellPadding="4" BorderWidth="1" BorderColor="#cccccc" GridLines="Both">
    <Columns>
        <asp:BoundField DataField="area_name" HeaderText="Area Name" SortExpression="area_name" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center" />
        <asp:BoundField DataField="CITY_PARENTS" HeaderText="City Parents" ReadOnly="True" SortExpression="CITY_PARENTS" ItemStyle-HorizontalAlign="Center" />
        <asp:BoundField DataField="KIRA" HeaderText="Kira" ReadOnly="True" SortExpression="KIRA" ItemStyle-HorizontalAlign="Center" />
        <asp:BoundField DataField="FAIRWAYS" HeaderText="Fairways" ReadOnly="True" SortExpression="FAIRWAYS" ItemStyle-HorizontalAlign="Center" />
        <asp:BoundField DataField="WINSTON" HeaderText="Winston" ReadOnly="True" SortExpression="WINSTON" ItemStyle-HorizontalAlign="Center" />
        <asp:BoundField DataField="MENGO" HeaderText="Mengo" ReadOnly="True" SortExpression="MENGO" ItemStyle-HorizontalAlign="Center" />
        <asp:BoundField DataField="KYENGERA" HeaderText="Kyengera" ReadOnly="True" SortExpression="KYENGERA" ItemStyle-HorizontalAlign="Center" />
        <asp:BoundField DataField="NAKASERO" HeaderText="Nakasero" ReadOnly="True" SortExpression="NAKASERO" ItemStyle-HorizontalAlign="Center" />
        <asp:BoundField DataField="OLD_KAMPALA" HeaderText="Old Kampala" ReadOnly="True" SortExpression="OLD_KAMPALA" ItemStyle-HorizontalAlign="Center" />
        <asp:BoundField DataField="MUGONGO" HeaderText="Mugongo" ReadOnly="True" SortExpression="MUGONGO" ItemStyle-HorizontalAlign="Center" />
        <asp:BoundField DataField="KISAASI" HeaderText="Kisaasi" ReadOnly="True" SortExpression="KISAASI" ItemStyle-HorizontalAlign="Center" />
        <asp:BoundField DataField="KITINTALE" HeaderText="Kitintale" ReadOnly="True" SortExpression="KITINTALE" ItemStyle-HorizontalAlign="Center" />
        <asp:BoundField DataField="Area_Average" HeaderText="Area Average" ReadOnly="True" SortExpression="Area_Average" ItemStyle-HorizontalAlign="Center" />
    </Columns>
</asp:GridView>
        <asp:SqlDataSource ID="piv" runat="server" ConnectionString="<%$ ConnectionStrings:s_cardConnectionString %>" SelectCommand="SELECT * FROM [finance_pivoted_view]"></asp:SqlDataSource>
        <br />
        <br />
        <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; RANKING<br />
        <br />
        <asp:GridView ID="GridView7" runat="server" AutoGenerateColumns="False" DataSourceID="rnk" EnableModelValidation="True">
            <Columns>
                <asp:BoundField DataField="school_name" HeaderText="school_name" SortExpression="school_name" />
                <asp:BoundField DataField="total_score" HeaderText="total_score" SortExpression="total_score" />
                <asp:BoundField DataField="average_score" HeaderText="average_score" SortExpression="average_score" />
                <asp:BoundField DataField="school_rank" HeaderText="school_rank" SortExpression="school_rank" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="rnk" runat="server" ConnectionString="<%$ ConnectionStrings:s_cardConnectionString %>" SelectCommand="SELECT * FROM [finance_ranking]"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>
        
        <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; WEEKLY ANALYSIS<br />
        <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:GridView ID="GridView9" runat="server" AutoGenerateColumns="False" DataSourceID="weekly" EnableModelValidation="True">
            <Columns>
                <asp:BoundField DataField="week_name" HeaderText="week_name" SortExpression="week_name" />
                <asp:BoundField DataField="department_name" HeaderText="department_name" SortExpression="department_name" />
                <asp:BoundField DataField="area_name" HeaderText="area_name" SortExpression="area_name" />
                <asp:BoundField DataField="school_name" HeaderText="school_name" SortExpression="school_name" />
                <asp:BoundField DataField="avg_score" HeaderText="avg_score" ReadOnly="True" SortExpression="avg_score" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="weekly" runat="server" ConnectionString="<%$ ConnectionStrings:s_cardConnectionString %>" SelectCommand="SELECT * FROM [week_analysis]"></asp:SqlDataSource>
        
        <br />
        <br />
        <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; WEEKLY COMPARISON<br />
        <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:GridView ID="GridView8" runat="server" AutoGenerateColumns="False" DataSourceID="wkly" EnableModelValidation="True">
            <Columns>
                <asp:BoundField DataField="Week1" HeaderText="Week1" SortExpression="Week1" />
                <asp:BoundField DataField="Week2" HeaderText="Week2" SortExpression="Week2" />
                <asp:BoundField DataField="area_name" HeaderText="area_name" SortExpression="area_name" />
                <asp:BoundField DataField="department_name" HeaderText="department_name" SortExpression="department_name" />
                <asp:BoundField DataField="school_name" HeaderText="school_name" SortExpression="school_name" />
                <asp:BoundField DataField="Week1_AvgScore" HeaderText="Week1_AvgScore" ReadOnly="True" SortExpression="Week1_AvgScore" />
                <asp:BoundField DataField="Week2_AvgScore" HeaderText="Week2_AvgScore" ReadOnly="True" SortExpression="Week2_AvgScore" />
                <asp:BoundField DataField="Score_Change" HeaderText="Score_Change" ReadOnly="True" SortExpression="Score_Change" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="wkly" runat="server" ConnectionString="<%$ ConnectionStrings:s_cardConnectionString %>" SelectCommand="SELECT * FROM [week_comparison]"></asp:SqlDataSource>
        
    </form>
</body>
</html>
