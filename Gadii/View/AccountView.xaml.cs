namespace Gadii.View;

public partial class AccountView : ContentPage
{
	public AccountView()
	{
		InitializeComponent();
        string htmlContent = @" <!DOCTYPE html> <html> <head> <script type='text/javascript' src='https://www.gstatic.com/charts/loader.js'></script> <script type='text/javascript'> google.charts.load('current', {packages:['corechart']}); function drawChart() { var listbox = document.getElementById('chart'); var selIndex = listbox.selectedIndex; var selValue = listbox.options[selIndex].value; var selText = listbox.options[selIndex].text; console.log(selValue); var goodvalue = 10; var badvalue = 20; if (selText== 'Today') { goodvalue = 20; badvalue = 30; } else if (selText == 'Current Week') { goodvalue = 20; badvalue = 40; } else if (selText == 'Current Month') { goodvalue = 40; badvalue = 30; } else if (selText == 'Current Year') { goodvalue = 20; badvalue = 30; } var data = google.visualization.arrayToDataTable([ ['Category', 'Percentage'], ['Good posture', goodvalue], ['Bad posture', badvalue] ]); var options = { title: 'Report', is3D: true, }; var chart = new google.visualization.PieChart(document.getElementById('piechart_3d')); chart.draw(data, options); } </script> </head> <body> <h2>Index</h2> <div> <h3>Employee</h3> <select id='chartEmp' name='ddlEmp'> <option selected disabled>Choose Employee</option> <option value='100'>Amol</option> <option value='200'>Eqbal</option> <option value='200'>Himani</option> <option value='200'>Pratik</option> </select> </div> <div> <h3>Range</h3> <select id='chart' name='ddlRange' onchange='drawChart()'> <option selected disabled>Choose Range</option> <option value='2'>Today</option> <option value='3'>Current Week</option> <option value='4'>Current Month</option> <option value='5'>Current Year</option> </select> </div> <div id='piechart_3d' style='width: 900px; height: 500px;'></div> </body> </html>"; 
		webView.Source = new HtmlWebViewSource { Html = htmlContent };
    }
}