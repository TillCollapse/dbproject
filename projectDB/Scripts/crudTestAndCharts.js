google.charts.load('current', { packages: ['corechart', 'bar'] });
var results = null;
$("#runTest").click(function () {
    $.ajax({
        url: "../../CRUD/RunCRUDTest",
        type: "GET",
        data: null,
        success: function (response) {
            results = response;
            google.charts.setOnLoadCallback(drawCharts);
            alert('OK');
        },
        error: function (response) {
            alert('BAD');
        }
    });
});

$("#checkItOut").click(function () {
    $.ajax({
        url: "../../CRUD/Count?value=" + $("#arename").val(),
        type: "GET",
        data: null,
        success: function (response) {
            results = response;
            $("#crimes_number").text(results);
            alert('OK');
        },
        error: function (response) {
            alert('BAD');
        }
    });
});

//function redirect() {
//    window.location.href = '/Home/StatisticsResults?' + $("#arename").val();
//}

function drawCharts() {
    //Create Test
    drawOneChart('Create', 'create_div', results.operationsNumbers, results.mongoCreate, results.mySQLCreate);
    //Read Test
    drawOneChart('Read', 'read_div', results.operationsNumbers, results.Read, results.mySQLRead);
    //Update Test
    //drawOneChart('Update', 'update_div', results.operationsNumbers, results.mongoCreate, results.mySQLCreate);
    //Delete Test
    //drawOneChart('Delete', 'delete_div', results.operationsNumbers, results.mongoCreate, results.mySQLCreate);
}

function drawOneChart(testName, divId, opeartionNumber, mongoResults, mySQLResults) {
    var data = new google.visualization.DataTable();
    data.addColumn('string', 'Operation Number');
    data.addColumn('number', 'Mongo');
    data.addColumn('number', 'MySQL');

    data.addRows([
        [{ v: opeartionNumber[0].toString(), f: 'Time' }, mongoResults[0], mySQLResults[0]],
        [{ v: opeartionNumber[1].toString(), f: 'Time' }, mongoResults[1], mySQLResults[1]],
        [{ v: opeartionNumber[2].toString(), f: 'Time' }, mongoResults[2], mySQLResults[2]],
        //[{v: "4", f: '11 am'}, results.mongoCreate[3], 2.25]
    ]);

    var optionsCreate = {
        chart: {
            title: 'Speed test '+ testName + ' operation',
            subtitle: ''
        },
        hAxis: {
            title: 'Number of operations',
            format: 'h:mm a',
            viewWindow: {
                min: [7, 30, 0],
                max: [17, 30, 0]
            }
        },
        vAxis: {
            title: 'Rating (scale of 1-10)'
        }
    };

    var material = new google.charts.Bar(document.getElementById(divId));
    material.draw(data, optionsCreate);
}

