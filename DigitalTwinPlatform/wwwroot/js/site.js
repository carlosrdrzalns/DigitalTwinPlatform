// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function getUseCases(modelId) {
    $.ajax({
        url: '/getUseCases',
        method: 'GET',
        data : {
            modelId : modelId
        },
        success: handleSuccess,
    })
}

function handleSuccess(data) {
    
    let panel = $('#useCasesList');
    data.forEach(function (uc) {
        let listElement = $('<div>');
        listElement[0].dataset.dbid = uc.id;
        listElement.text(uc.caseName);
        listElement.addClass('list-element');
        panel.append(listElement);
        listElement.on('click', handleUseCaseClick)
    });
}

function handleUseCaseClick(event) {
    let target = $(this);
    let dashboardPanel = $('#dashboardPanel');
    let id = target[0].dataset.dbid;
    if (!dashboardPanel.hasClass('hidden')) {
        if (dashboardPanel[0].dataset.dbid == id) {
            dashboardChanged(false);
            dashboardPanel[0].dataset.dbid = '';
            viewer.restoreState(originalState, null, false);
            return
        }
        else {
            dashboardPanel[0].dataset.dbid = id;
            dashboardPanel.empty();
        }
    } else {
        dashboardChanged(true);
        dashboardPanel[0].dataset.dbid=  id;
    }
    let LateralPanel = $('<div>');
    LateralPanel.addClass('LateralPanelNew');
    let LateralPanelHeader = $('<div>');
    LateralPanelHeader.addClass('LateralPanelHeader');
    LateralPanelHeader.append($('<span>Caso de Uso:'+"\t"+'</span>')).append($('<span>' + target.text() + '</span>'))
    LateralPanel.append(LateralPanelHeader);
    dashboardPanel.append(LateralPanel);
    $.ajax({
        url: '/getUseCase',
        method: 'GET',
        data: { id: id }, 
        success: (data) => {
            let state = JSON.parse(data.state);
            viewer.restoreState(state, null, false);
        }
    })
    if (target.text() == "Bombeo") {
        BombeoCharts();
    } else {
        ReactorCharts();
    }
}

Number.prototype.padLeft = function (base, chr) {
    var len = (String(base || 10).length - String(this).length) + 1;
    return len > 0 ? new Array(len).join(chr || '0') + this : this;
}

async function BombeoCharts() {
    let myChart;
    let PanelBody = $('<div>');
    PanelBody.addClass('LateralPanelBody');
    let PredictionChartDiv = $('<div>');
    let PredictionCanvas = $('<canvas>');
    PredictionCanvas.attr('id', 'PredictionChart');
    let contextPrediction = PredictionCanvas.get(0).getContext('2d');
    
    PredictionChartDiv.append(PredictionCanvas);
    PanelBody.append(PredictionChartDiv);
    $('#dashboardPanel .LateralPanelNew').append(PanelBody);
    let height = PanelBody.height();
    PredictionCanvas.height(height / 2);
    setInterval(function () {
        $.ajax({
            url: '/getPredictionData',
            method: 'GET',
            dataType: 'json',
            data: {
                nElements: 10
            },
            success: function (newData) {
                let timestamp = newData.map(function (entry) {
                    d = new Date(entry.timestamp),
                        dformat= d.toLocaleString()
                    return dformat
                });
                timestamp.sort((time1, time2) => {
                    time1 = time1.split(",")[1]
                    time2 = time2.split(",")[1]
                    const [h1, m1, s1] = time1.split(":").map(Number);
                    const [h2, m2, s2] = time2.split(":").map(Number);

                    if (h1 !== h2) {
                        return h1 - h2;
                    } else if (m1 !== m2) {
                        return m1 - m2;
                    } else {
                        return s1 - s2;
                    }
                });
                let predictions = newData.map(entry => entry.status);
                if (myChart) {
                    // If the chart instance already exists, update its data
                    myChart.data.labels=timestamp;
                    myChart.data.datasets[0].data=predictions;
                    myChart.update();
                } else {
                    // If the chart instance doesn't exist, create a new chart
                    myChart = new Chart(contextPrediction, {
                        type: 'line',
                        data: {
                            labels: timestamp,
                            datasets: [{
                                label: 'Predictions',
                                data: predictions,
                                borderColor: 'blue',
                                fill: false
                            }]
                        },
                        options: {
                            responsive: true,
                            maintainAspectRatio: false,
                            scales: {
                                y: {
                                    max: 1.5,
                                    min: -0.5,
                                    beginAtZero: true
                                }
                            },
                            plugins: {
                                legend: {
                                    display: false,
                                },
                                title: {
                                    display: true,
                                    text: "Water Pump Predictions",
                                    position: "bottom",
                                    color: '#ABAAAA',
                                }
                            }                            
                        }
                    });
                }
            }
        });
    }, 1000);
}



function ReactorCharts() {
    let myChart;
    let PanelBody = $('<div>');
    PanelBody.addClass('LateralPanelBody');
    let PredictionChartDiv = $('<div>');
    let PredictionCanvas = $('<canvas>');
    PredictionCanvas.attr('id', 'PredictionChart');
    let contextPrediction = PredictionCanvas.get(0).getContext('2d');

    PredictionChartDiv.append(PredictionCanvas);
    PanelBody.append(PredictionChartDiv);
    $('#dashboardPanel .LateralPanelNew').append(PanelBody);
    let height = PanelBody.height();
    PredictionCanvas.height(height);
    setInterval(function () {
        $.ajax({
            url: '/getReactorData',
            method: 'GET',
            dataType: 'json',
            data: {
                nElements: 3
            },
            success: function (newData) {
                let timestamp = newData.map(function (entry) {
                    d = new Date(entry.timestamp),
                        dformat = d.toLocaleString()
                    return dformat
                });
                timestamp.sort((time1, time2) => {
                    time1 = time1.split(",")[1]
                    time2 = time2.split(",")[1]
                    const [h1, m1, s1] = time1.split(":").map(Number);
                    const [h2, m2, s2] = time2.split(":").map(Number);

                    if (h1 !== h2) {
                        return h1 - h2;
                    } else if (m1 !== m2) {
                        return m1 - m2;
                    } else {
                        return s1 - s2;
                    }
                });
                let data_in = [];
                let data_r1 = [];
                let data_r2 = [];
                newData.forEach(function (datas) {                   
                    for (var name in datas) {
                        if (name == 'timestamp' || name =='id') continue;
                        if (name[name.length - 1] == 0) {
                            if (data_in.find(item => item.label === name)) {
                                data_in.find(item => item.label === name).data.push(datas[name]);
                            } else {
                                data = {
                                    label: name,
                                    data: [datas[name]]
                                };
                                data_in.push(data);
                            }
                        }
                        else if (name.includes("ini2")) {
                            if (data_r2.find(item => item.label === name)) {
                                data_r2.find(item => item.label === name).data.push(datas[name]);
                            } else {
                                data = {
                                    label: name,
                                    data: [datas[name]]
                                };
                                data_r2.push(data);
                            }
                        }
                        else {
                            if (data_r1.find(item => item.label === name)) {
                                data_r1.find(item => item.label === name).data.push(datas[name]);
                            } else {
                                data = {
                                    label: name,
                                    data: [datas[name]]
                                };
                                data_r1.push(data);
                            }
                        }
                    }
                });
                if (myChart) {
                    // If the chart instance already exists, update its data
                    myChart.data.labels = timestamp;
                    myChart.data.datasets = data_r2;
                    myChart.update();
                } else {
                    // If the chart instance doesn't exist, create a new chart
                    myChart = new Chart(contextPrediction, {
                        type: 'line',
                        data: {
                            labels: timestamp,
                            datasets:  data_r2
                        },
                        options: {
                            responsive: true,
                            maintainAspectRatio: false,
                            scales: {
                                y: {
                                    max: 0,
                                    min: 1500,
                                    beginAtZero: true
                                }
                            },
                            plugins: {
                                legend: {
                                    display: true,
                                    position: "right"
                                },
                                title: {
                                    display: true,
                                    text: "Biological Reactor Data",
                                    position: "bottom",
                                    color: '#ABAAAA',
                                }
                            }                         
                        }
                    });
                }
            }
        });
    }, 1000);
}


function dashboardChanged(bool) {
    if (bool) {
        $('#viewerPanel').removeClass('col-sm-10 transition-width').addClass('col-sm-6 transition-width');
        $('#dashboardPanel').removeClass('hidden').addClass('col-sm-4 transition-width');
        $('#viewerPanel').one("webkitTransitionEnd oTransitionEnd MSTransitionEnd", function () {
            viewer.resize();
            viewer.fitToView();
        });
    } else
    {
        $('#viewerPanel').removeClass('col-sm-6 transition-width').addClass('col-sm-10 transition-width');
        $('#dashboardPanel').removeClass('col-sm-6 transition-width');
        $('#dashboardPanel').empty();
        $('#viewerPanel').one("webkitTransitionEnd oTransitionEnd MSTransitionEnd", function () {
            viewer.resize();
            viewer.fitToView();
            $('#dashboardPanel').addClass('hidden')
        });
    }
}



