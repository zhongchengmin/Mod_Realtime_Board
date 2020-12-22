<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="Mod_Realtime_Board.Forms.HomePage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>MOD3 Realtime Line Product Board</title>
    <link href="../Styles/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/HomePage.css" rel="stylesheet" type="text/css" />

     <script src="../Scripts/vue.js" type="text/javascript"></script>
    <script src="../Scripts/elementUI.js" type="text/javascript"></script>
    <script src="../Scripts/echarts.min.js" type="text/javascript"></script>
    <script src="../Scripts/axios.min.js" type="text/javascript"></script>
    

</head>
<body style="height: 110%; width: 100%">
    <div id="app">
        <div style="height: 110%">
            <table runat="server" id="table0" style="height: 8.64%; width: 100%;">
                <tr style="height: 2.78%; width: 100%" align="center" valign="bottom">
                    <td colspan="19" rowspan="6" style="width: 100%;" align="center" valign="bottom">
                        <asp:Label ID="lblTitle" runat="server" Font-Bold="True" Font-Names="Microsoft YaHei"
                            Font-Size="55pt" ForeColor="Blue" Text="LCM3 Realtime Line Product Board"></asp:Label>
                    </td>
                </tr>
                <tr style="height: 2.78%">
                </tr>
                <tr style="height: 2.78%">
                </tr>
                <tr style="height: 2.78%">
                </tr>
                <tr style="height: 2.78%">
                </tr>
                <tr style="height: 2.78%">
                </tr>
                <tr>
                    <td align="left" valign="top" colspan="19">
                        <asp:Label ID="lblShowTime" runat="server" Font-Bold="True" Font-Names="Microsoft YaHei"
                            Font-Size="8pt" ForeColor="#8080FF">{{refreshTimeInfo}}</asp:Label>
                    </td>
                </tr>
            </table>
            <table id="tableMain" runat="server" cellpadding="0" cellspacing="0" style="table-layout: fixed;
                border-right: black thin solid; border-top: black thin solid; border-left: black thin solid;
                border-bottom: black thin solid; height: 101.36%; width: 100%;" width="100%">
                <tr style="height: 0.01%; width: 100%" align="center">
                    <td colspan="19" rowspan="1" style="height: 0.01%; width: 100%">
                    </td>
                </tr>
                <tr style="height: 2.78%; width: 100%; border-top: black thin solid; border-left: black thin solid;
                    border-right: black thin solid;">
                    <td colspan="4" rowspan="2" style="height: 5.56%; width: 22.22%; border-top-width: thin;
                        border-bottom-width: thin; border-bottom-color: black; border-top-color: black;">
                        <asp:Image ID="imgLogo" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Photo/logo.bmp"
                            Width="250px" Height="60px" />
                    </td>
                   
                    <td colspan="2" rowspan="2" style="height: 5.56%; width: 22.22%; border-top-width: thin;
                        border-bottom-width: thin; border-bottom-color: black; border-top-color: black;">
                        <asp:Label ID="lblMOD" runat="server" Font-Bold="True" Text="廠別：MOD3" Font-Size="16pt"
                            ForeColor="Black"></asp:Label>
                    </td>
                    <td colspan="2" style="height: 5.56%; width: 22.22%; border-top-width: thin; border-bottom-width: thin;
                        border-bottom-color: black; border-top-color: black;" rowspan="2">
                        <asp:Label ID="lblShiftName" runat="server" Font-Bold="True" ForeColor="Black"
                            Font-Size="16pt">{{'班別：'+justifyShift}}</asp:Label>
                    </td>
                     <td colspan="2" style="height: 5.56%; width: 22.22%; border-top-width: thin; border-bottom-width: thin;
                        border-bottom-color: black; border-top-color: black;" rowspan="2">
                        <asp:Label ID="lblProcess" runat="server" Font-Bold="True" Text="製程：" ForeColor="Black"
                            Font-Size="16pt"></asp:Label>
                            <select class="form-control procSelect" ref="process" v-on:change="proSelect($event.target.value)">
                              <option value="BONDING">BONDING</option>
                              <option value="CDKEN">CDKEN</option>
                              <option value="ASSY">ASSY</option>
                              <option value="LAM">LAM</option>
                            </select>
                    </td>
                     <td runat="server" id="tdSublineCmt" colspan="2" rowspan="2" style="height: 5.56%;
                        width: 22.22%; border-top-width: thin; border-left-width: thin; border-left-color: black;
                        border-top-color: black; border-right-width: thin;
                        border-right-color: black;" align="center" valign="middle">
                        <span class="line">線別：</span>
                        <select class="form-control lineSelect" ref="line">
                        <option  v-for="item in line" v-bind:value="item.EQP_LINE">{{item.EQP_LINE}}</option>
                        </select>
                    </td>
                    <td colspan="7" style="height: 5.56%; width: 22.22%; border-top-width: thin; border-bottom-width: thin;
                        border-bottom-color: black; border-top-color: black;">
                        <asp:Label ID="lblLeaveQty" runat="server" Font-Bold="True" Text="" Font-Size="16pt"
                            ForeColor="Black"></asp:Label>
                    </td>
                </tr>
                <tr style="height: 5.56%;border-left: black thin solid;">
                    <td colspan="1" rowspan="1" style="height: 5.56%; border-top-width: thin; border-bottom-width: thin;
                        width: 22.22%;" align="center"
                        valign="bottom">
                        <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="產量" ForeColor="Blue"></asp:Label>
                    </td>
                    <td colspan="1" rowspan="1" style="height: 5.56%; border-top-width: thin; border-bottom-width: thin;
                        width: 22.22%;" align="center"
                        valign="bottom">
                        <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="品質" ForeColor="Blue"></asp:Label>
                    </td>
                    <td rowspan="1" style="height: 5.56%; border-top-width: thin; border-bottom-width: thin;
                         width: 149px;" colspan="1"
                        align="center" valign="bottom">
                        <asp:Label ID="Label3" runat="server" Font-Bold="True" Text="設備" ForeColor="Blue"></asp:Label>
                    </td>
                    <td rowspan="3" colspan="4" style="height: 5.56%; border-top-width: thin; width: 149px;
                        border-top-color: black; border-bottom: black thin solid;" align="left" valign="bottom">
                        <asp:Image ID="imgLightCmt" runat="server" BorderStyle="None" ImageUrl="~/Photo/燈號說明.JPG"
                            BorderColor="#8080FF" BorderWidth="0px" Height="80px" Width="100%" />
                    </td>
                </tr>
                <tr style="height: 5.56%">
                    <td rowspan="2" colspan="4" style="border-bottom: black thin solid; height: 6%">
                        <asp:Label ID="lblSublineID" runat="server" Font-Bold="True" Font-Names="Microsoft YaHei"
                            Font-Size="34pt" ForeColor="Black">{{lineShow}}</asp:Label>
                        &nbsp;&nbsp;
                    </td>
                    <td colspan="2" rowspan="2" style="height: 5.56%; border-top-width: thin; width: 22.22%;
                        border-top-color: black; border-bottom: black thin solid;">
                        <asp:Label ID="lblLineOwner" runat="server" Font-Bold="True" Text="線長：" Font-Size="16pt"
                            ForeColor="Black"></asp:Label>
                    </td>
                    <td colspan="4" rowspan="2" style="height: 5.56%; border-top-width: thin; width: 22.22%;
                        border-top-color: black; border-bottom: black thin solid;">
                        <asp:Label ID="lblProdID" runat="server" Font-Bold="True"
                            Font-Size="16pt" ForeColor="Black">{{"機種："+currentProd}}</asp:Label>
                    </td>
                    <td colspan="2" rowspan="2" style="height: 5.56%; border-top-width: thin; width: 22.22%;
                        border-bottom: black thin solid;text-align:center;">
                        <button type="button" class="btn btn-success" v-on:click="query">查詢</button>
                    </td>
                    <td rowspan="2" colspan="1" style="border-bottom: black thin solid; height: 6%" align="center"
                        valign="middle">
                        <asp:Image ID="imgLightQty" runat="server" BorderStyle="None" ImageUrl="~/Photo/Light-Green.jpg"
                            BorderColor="#8080FF" BorderWidth="0px" Height="50px" Width="50px" />
                    </td>
                    <td rowspan="2" colspan="1" style="border-bottom: black thin solid; height: 6%" align="center"
                        valign="middle">
                        <asp:Image ID="imgLightYield" runat="server" BorderStyle="None" ImageUrl="~/Photo/Light-Red.jpg"
                            Height="50px" Width="50px" />
                    </td>
                    <td rowspan="2" style="border-bottom: black thin solid; height: 6%; width: 149px;"
                        align="center" valign="middle">
                        <asp:Image ID="imgLightEQ" runat="server" BorderStyle="None" ImageUrl="~/Photo/Light-Yellow.jpg"
                            Height="50px" Width="50px" />&nbsp;
                    </td>
                </tr>
                <tr style="height: 5.56%">
                </tr>
                <tr style="height: 2.78%">
                    <td colspan="2" style="height: 2.78%; width: 5.56%;">
                    </td>
                    <td rowspan="3" colspan="4" style="border-bottom: black thin solid; height: 8.34%;
                        width: 5.56%; border-right: black thin solid;">
                        <asp:Image runat="server" AlternateText="暫無" ID="imgMFGOwner" ImageAlign="Left" Height="112px" ImageUrl="~/Photo/007.bmp"
                            Width="85px" />
                    </td>
                    <td colspan="3" align="center" style="height: 2.78%; width: 11.12%;">
                        <asp:Label ID="lblManpowerStandardQty" runat="server" BackColor="YellowGreen" BorderStyle="Outset"
                            Font-Bold="False" Font-Size="12pt" ForeColor="Black"
                            Width="80%">{{"標準人力&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"+pgInfo.OLD_NEED}}</asp:Label>
                    </td>
                    <td colspan="3" style="border-top-width: thin; border-top-color: black; border-right: black thin solid;
                        height: 2.78%; width: 11.12%;" align="center">
                        <asp:Label ID="lblOutputTragetQty" runat="server" BackColor="YellowGreen" BorderStyle="Outset"
                            Font-Bold="False" Font-Size="12pt" ForeColor="Black" Text=""
                            Width="80%">{{"產出目標&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"+outputTarget}}</asp:Label>
                    </td>
                    <td colspan="2" style="height: 2.78%;">
                    </td>
                    <td rowspan="3" colspan="5" style="border-bottom: black thin solid; height: 8.34%;">
                        <asp:Image runat="server" ID="imgProcessOwner" AlternateText="暫無" ImageAlign="Left" ImageUrl="~/Photo/007.bmp"
                            Height="112px" Width="85px" />
                    </td>
                </tr>
                <%--                <tr style="height: 2.78%">
                </tr>--%>
                <tr style="height: 2.78%">
                    <td colspan="1" style="border-bottom: black thin solid; height: 5.56%;" rowspan="2"
                        align="left" valign="top">
                        <asp:Label ID="lblMFGOwnerTitle" runat="server" Font-Bold="True" Font-Size="16pt"
                            ForeColor="Black" Text="線長" Font-Names="標楷體"></asp:Label>
                    </td>
                    <td style="border-bottom: black thin solid; height: 5.56%;" rowspan="2" align="left"
                        valign="top">
                        <asp:Label ID="lblMFGOwnerName" runat="server" Font-Bold="True" Font-Size="16pt"
                            Text="" Font-Names="標楷體" ForeColor="Black"></asp:Label>
                    </td>
                    <td colspan="3" align="center" valign="middle" style="height: 2.78%;">
                        <asp:Label ID="lblManpowerAutalQty" runat="server" BackColor="SteelBlue" BorderStyle="Outset"
                            Font-Bold="False" Font-Size="12pt" ForeColor="Black"
                            Width="80%">{{"實際人力&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"+pgInfo.QTY}}</asp:Label>
                    </td>
                    <td colspan="3" style="border-right: black thin solid; height: 2.78%; width: 11.12%;"
                        align="center" valign="middle">
                        <asp:Label ID="lblOutputActualQty" runat="server" BackColor="SteelBlue" BorderStyle="Outset"
                            Font-Bold="False" Font-Size="12pt" ForeColor="Black"
                            Width="80%">{{"實際產出&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"+outputActual}}</asp:Label>
                    </td>
                    <td rowspan="2" colspan="1" style="border-bottom: black thin solid; height: 5.56%;"
                        align="left" valign="top">
                        <asp:Label runat="server" ID="lblProcessOwnerText" Text="製程" Font-Bold="True" ForeColor="Black"
                            Font-Size="16pt" Font-Names="標楷體"></asp:Label>
                    </td>
                    <td rowspan="2" style="border-bottom: black thin solid; height: 5.56%" align="left"
                        valign="top">
                        <asp:Label runat="server" ID="lblProcessOwnerName" Text="" Font-Bold="True" Font-Size="16pt"
                            Font-Names="標楷體" ForeColor="Black"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="border-bottom: black thin solid; height: 2.78%; width: 11.12%"
                        align="center" valign="middle">
                        <asp:Label runat="server" ID="lblManpowerDiffQty" Text="差異人力&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;0"
                            Width="80%" ForeColor="Black" BackColor="LightSalmon" Font-Bold="False" Font-Size="12pt"
                            BorderStyle="Outset">{{"差異人力&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"+(pgInfo.QTY-pgInfo.OLD_NEED).toFixed(1)}}</asp:Label>
                    </td>
                    <td colspan="3" style="border-bottom: black thin solid; height: 2.78%; border-right: black thin solid;
                        width: 11.12%;" align="center" valign="middle">
                        <asp:Label ID="lblOutputDiffQty" runat="server" BackColor="LightSalmon" BorderStyle="Outset"
                            Font-Bold="False" Font-Size="12pt" ForeColor="Black" Text="產出差異&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;-16611"
                            Width="80%">{{"產出差異&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"+(outputActual-outputTarget)}}</asp:Label>
                    </td>
                </tr>
                <%--                <tr style="height: 2.78%">
                </tr>
                <tr style="height: 2.78%">
                </tr>
                <tr style="height: 2.78%">
                </tr>--%>
                <tr style="height: 2.78%">
                    <td rowspan="1" colspan="6" style="border-right: black thin solid; border-top-width: thin;
                        border-left-width: thin; border-left-color: black; border-top-color: black; border-bottom-width: thin;
                        border-bottom-color: black;padding-right:10px;">
                        <asp:Label runat="server" ID="lblChartTitle1" Text="&nbsp;&nbsp;分時產出&nbsp;&nbsp;"
                            BackColor="Blue" Font-Bold="True" ForeColor="White" BorderStyle="Outset" Font-Names="標楷體"
                            Font-Size="14pt"></asp:Label>

                        <div id="hourOutputChart" ref="hourOutputChart">
                            
                        </div>
                    </td>
                    <td rowspan="1" colspan="6" style="border-right: black thin solid; border-top-width: thin;
                        border-left-width: thin; border-left-color: black; border-top-color: black; border-bottom-width: thin;
                        border-bottom-color: black;padding-right:10px;">
                        <asp:Label ID="Label4" runat="server" BackColor="Blue" BorderStyle="Outset" Font-Bold="True"
                            ForeColor="White" Text="&nbsp;&nbsp;累計產出&nbsp;&nbsp;" Font-Names="標楷體" Font-Size="14pt"></asp:Label>
                        <div id="totalOutputChart" ref="totalOutputChart">
                            
                        </div>
                    </td>
                    <td rowspan="1" colspan="7" style="border-top-width: thin; border-top-color: black;
                        border-right-width: thin; border-right-color: black; border-left-width: thin;
                        border-left-color: black; border-bottom-width: thin; border-bottom-color: black;">
                        <asp:Label ID="Label5" runat="server" BackColor="Blue" BorderStyle="Outset" Font-Bold="True"
                            ForeColor="White" Text="&nbsp;&nbsp;分時良率&nbsp;&nbsp;" Font-Names="標楷體" Font-Size="14pt"></asp:Label>

                        <div id="hourRateChart" ref="hourRateChart">
                            
                        </div>
                    </td>
                </tr>
                <tr>
                    <td rowspan="15" colspan="6" style="border-right: black thin solid; border-bottom: black thin solid;
                        border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black;">
                        
                    </td>
                    <td rowspan="15" colspan="6" style="border-right: black thin solid; border-bottom: black thin solid;
                        border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black;">
                        
                    </td>
                    <td rowspan="15" colspan="7" style="border-bottom: black thin solid; border-top-width: thin;
                        border-top-color: black; border-right-width: thin; border-right-color: black;
                        border-left-width: thin; border-left-color: black;">
                        
                    </td>
                </tr>
                <tr style="height: 2.78%">
                </tr>
                <tr style="height: 2.78%">
                </tr>
                <tr style="height: 2.78%">
                </tr>
                <tr style="height: 2.78%">
                </tr>
                <tr style="height: 2.78%">
                </tr>
                <tr style="height: 2.78%">
                </tr>
                <tr style="height: 2.78%">
                </tr>
                <tr style="height: 2.78%">
                </tr>
                <tr style="height: 2.78%">
                </tr>
                <tr style="height: 2.78%">
                </tr>
                <tr style="height: 2.78%">
                </tr>
                <tr style="height: 2.78%">
                </tr>
                <tr style="height: 2.78%">
                </tr>
                <tr style="height: 2.78%">
                </tr>
                <tr style="height: 2.78%">
                    <td rowspan="1" colspan="6" style="border-right: black thin solid; border-top-width: thin;
                        border-left-width: thin; border-left-color: black; border-top-color: black; border-bottom-width: thin;
                        border-bottom-color: black;">
                        <asp:Label runat="server" ID="Label7" Text="&nbsp;&nbsp;設備狀態&nbsp;&nbsp;" BackColor="Blue"
                            Font-Bold="True" ForeColor="White" BorderStyle="Outset" Font-Names="標楷體" Font-Size="14pt"></asp:Label>
                    </td>
                    <td rowspan="3" colspan="13" style="border-top-width: thin; border-left-width: thin;
                        border-left-color: black; border-top-color: black; border-bottom-width: thin;
                        border-bottom-color: black; border-right-width: thin; border-right-color: black;">
                       <div>
                       
                       </div>
                    </td>
                </tr>
                <tr style="height: 2.78%;" align="center">
                    <td rowspan="2" colspan="1" style="border-top-width: thin; border-left-width: thin;
                        border-left-color: black; border-top-color: black; height: 5.56%;" align="left"
                        valign="middle">
                        <asp:Label runat="server" ID="lblEQOwnerText" Text="設備" Font-Bold="True" ForeColor="Black"
                            Font-Size="16pt" Font-Names="標楷體"></asp:Label>
                    </td>
                    <td rowspan="2" colspan="1" style="border-top-width: thin; border-left-width: thin;
                        border-left-color: black; border-top-color: black;" align="left" valign="middle">
                        <asp:Label runat="server" ID="lblEQOwnerName" Text="" Font-Bold="True" Font-Size="16pt"
                            Font-Names="標楷體" ForeColor="Black"></asp:Label>
                    </td>
                    <td rowspan="2" colspan="4" style="border-left-width: thin; border-left-color: black;
                        border-top-width: thin; border-top-color: black; border-right: black thin solid;">
                        <asp:Image runat="server" ID="imgEQOwner" AlternateText="暫無" ImageAlign="Left" Height="112px" ImageUrl="~/Photo/007.bmp"
                            Width="85px" />
                    </td>
                </tr>
                <tr style="height: 2.78%">
                </tr>
            </table>
        </div>
    </div>
    <script>
        var app = new Vue({
            el: '#app',
            data: function () {
                return {
                    line: [],//線別下拉框數據
                    lineShow:'',//左上角的製程段+線別
                    currentProd:'',//機種
                    outputTarget:0,//產出目標
                    outputActual:0,//產出實際
                    refreshTimeInfo:'',//刷新時間信息
                    refreshTime:900,//刷新時間
                    pgInfo:{},//派工數據
                    Option:{
                        title: {
                            show:false,
                            text: '',
                            x: "center",
                            y: "top",
                            textStyle: {
                                color: '#FF8800'
                            }
                        },
                        grid:{
                            show:false,
                            left:0,
                            bottom:2,
                             containLabel:true,
                             width:'100%',
                             height:'85%'
                        },
                        tooltip: {
                            show: true,
                            axisPointer:{
                                type:'cross'
                            },
                            formatter:function(result){
                                return '時間：'+result.name+'<br/>'+result.seriesName+'：'+result.value+''
                            }
                        },
                        animation: {

                        },
                        toolbox: {
                            show: true,
                            orient: 'horizontal',      // 布局方式，默认为水平布局，可选为：
                            // 'horizontal' ¦ 'vertical'
                            top: 'top',
                            right: 0,
                            //backgroundColor: '#fff', // 工具箱背景颜色
                            borderColor: '#ccc',       // 工具箱边框颜色
                            borderWidth: 0,            // 工具箱边框线宽，单位px，默认为0（无边框）
                            padding: 5,                // 工具箱内边距，单位px，默认各方向内边距为5，
                            showTitle: true,
                            feature: {
                                dataView: {
                                    show: false,
                                    title: '数据视图',
                                    readOnly: true,
                                    lang: ['数据视图', '关闭', '刷新'],
                                    optionToContent: function () {
                                        var table='';

                                        return table;
                                    }
                                },
                                magicType: {
                                    show: true,
                                    title: {
                                        line: '动态类型切换-折线图',
                                        bar: '动态类型切换-柱形图',
                                    },
                                    type: ['line','bar']
                                },
                                restore: {
                                    show: true,
                                    title: '还原',
                                    color: 'black'
                                },
                                saveAsImage: {
                                    show: true,
                                    title: '保存为图片',
                                    type: 'jpeg',
                                    lang: ['点击本地保存']
                                }
                            }
                        },
                        legend: {
                             show:true,
                            data: ['計劃','實際'],
                            orient: 'horizontal',
                            left: 'left', 
                            top: 'top',
                            bottom:'10',
                            textStyle: {
                                color: '#000',
                                fontSize: '12'
                            }//設置圖例的字體顏色和大小
                        },
                        xAxis: {
                            type:'category',
                            data: ["0730", "0830", "0930", "1030", "1130", "1230","1330","1430","1530","1630","1730","1830"],
                            boundaryGap:true,
//                            axisTick:{
//                                interval:0
//                            },
                            axisLabel:{
                                interval:0
                            }
                        },
                        yAxis: {
                            type:'value',
                            scale:true,
                            splitLine: {
                                show: false//是否显示分隔线。默认数值轴显示，类目轴不显示。
                            }
                        },
                        series: [{
                            name: '實際',
                            type: 'bar',
                            barWidth:'70%',
                            barMaxWidth:70,
                            data: [58, 69, 85, 52,40, 20,35,75,10,25,65,14],
                            label:{
                                show:true,
                                position :'top',
                                textStyle: {
                                    color: '#555'
                                },
                                formatter:function(val){
                                    if(val.value==0){
                                        return ''
                                    }else{
                                        return val.value
                                    }
                                }
                            }
                        },
                        {
                            name: '計劃',
                            type: 'line',
                            data: [58, 69, 85, 52,40, 20,35,75,10,25,65,14],
                            itemStyle: {
                                color:'red'
                            },
                            label:{
                                show:true,
                                position :'top'
                            }
                        }
                        ]
                    }
                }
            },
            created: function () {

            },
            computed:{
                justifyShift:function(){
                    var date = new Date();
                    var hour = date.getHours();
                    if(hour>=8&&hour<=20){
                        return '白班';
                    }else{
                        return '晚班';
                    }
                }
            },
            mounted: function () {
                var process = this.$refs.process.value;
                this.loadFirstData(process);
            },
            methods: {
                //第一次加載數據
                loadFirstData:async function(process){
                    var lineResponseData=await axios.get('HomePage.aspx', { params: { 'flag': 'line', 'process': process} });
                   if(lineResponseData.data.data.length>0){
                        this.line=lineResponseData.data.data;
                        this.lineShow = this.$refs.process.value+"-"+lineResponseData.data.data[0]["EQP_LINE"];
                    }
                    this.$nextTick(()=>{
                        this.loadQueryData(process,this.$refs.line.value);
                    });
                },
                loadQueryData:function(process,line){
                    
                    //分時產出
                    this.hourOutput(process,line);

                    //分時匯總產出邏輯
                    this.totalOutput(process,line);

                    //分時良率邏輯
                    this.hourRate(process,line);
                    
                    //派工邏輯
                    this.pgQty(process,line);

                },
                //分時產出邏輯
                hourOutput:async function(process,line){
                    var hourOutput=await axios.get('HomePage.aspx',{params:{'flag':'hourOutput','process':process,'line':line}});
                    var hourOutputChart = echarts.init(this.$refs.hourOutputChart);

                    var hourOutputChart_xAxis=[];//橫坐標
                    var hourOutputChart_yAxis=[];//縱坐標
                    var hourOutputChart_target=[];//目標

                    var barColor='';

                    if(hourOutput.data.data.length>0){
                        hourOutput.data.data.forEach((item,index)=>{
                        if(parseFloat(item['RATE'])<0.98){
                            barColor='rgb(195, 75, 75)';
                        }else if(parseFloat(item['RATE'])>=1){
                            barColor='yellowgreen';
                        }else{
                            barColor='rgb(255, 145, 45)';
                        }
                            hourOutputChart_xAxis.push(item['TIMEINTERVAL']);
                            hourOutputChart_yAxis.push({
                            value:item['QTY'],
                            itemStyle: {
                                normal:{
                                    color:barColor
                                }
                            }
                            }
                            );
                            hourOutputChart_target.push(item['PLANT_QTY_BYHOUR']);

                            //獲取當前機種
                            var date=new Date();
                            var hour = date.getHours();
                            if(hour+1==parseInt(item['TIMEINTERVAL'].substr(0,2))){
                                this.currentProd=item['PROD_NBR'];
                            }
                        });
                    }
                    this.Option.xAxis.data=hourOutputChart_xAxis;
                    this.Option.series[0].data=hourOutputChart_yAxis;
                    this.Option.series[1].data=hourOutputChart_target;
                    hourOutputChart.setOption(this.Option);  
                },
                //分時總產出邏輯
                totalOutput:async function(process,line){
                  var totalOutput=await axios.get('HomePage.aspx',{params:{'flag':'totalOutput','process':process,'line':line}});
                    var totalOutputChart = echarts.init(this.$refs.totalOutputChart);

                    var totalOutputChart_xAxis=[];//橫坐標
                    var totalOutputChart_yAxis=[];//縱坐標
                    var totalOutputChart_target=[];//目標

                    if(totalOutput.data.data.length>0){
                        //獲取當班最後匯總的目標和實際
                        this.outputTarget=totalOutput.data.data[totalOutput.data.data.length-1]['PLANT_QTY_BYHOUR'];
                        this.outputActual=totalOutput.data.data[totalOutput.data.data.length-1]['QTY'];

                        totalOutput.data.data.forEach(function(item,index){
                            totalOutputChart_xAxis.push(item['TIMEINTERVAL']);
                            totalOutputChart_yAxis.push(item['QTY']);
                            totalOutputChart_target.push(item['PLANT_QTY_BYHOUR']);
                        });
                    }else{
                        this.outputTarget=0;
                        this.outputActual=0;
                    }
                    this.Option.xAxis.data=totalOutputChart_xAxis;
                    this.Option.series[0]={
                            name: '實際',
                            type: 'line',
                            data: totalOutputChart_yAxis,
                            itemStyle: {
                                color:'blue'
                            },
                            label:{
                                show:true,
                                position :'top',
                                textStyle: {
                                    color: '#555'
                                }
                            }
                        }
                    this.Option.series[1].data=totalOutputChart_target;
                    totalOutputChart.setOption(this.Option);
                    this.Option.series[0]={};
                },//分時良率(串接分時良率這個邏輯大概花了一天的時間，因為之前很少做堆疊圖，按照堆疊圖的規則來整理數據話費了漫長的時間。主要是需要從獲取到的數據進行加工和整理以滿足堆疊圖數據的呈現)
                //可以從這個網址觀察堆疊圖數據的規則來整理數據。https://echarts.apache.org/examples/zh/editor.html?c=bar-stack
                //整理數據的主要難點是在堆疊圖的縱坐標這一方面，首先需要根據每個defect獲取每個時間段的所有defect數量，如果在該時間段有defect則計算出在改時間段的數量否則數量為0，然後對數據進行去重處理，如果同一個時間段的同一個defect有多個，如果數量都為0則取一筆，否則就取不為0的那筆，這大概就是複雜的邏輯
                hourRate:async function(process,line){
                     var hourRate=await axios.get('HomePage.aspx',{params:{'flag':'rate','process':process,'line':line}});

                     var hourRateChart_xAxis=[];//橫坐標
                     var hourRateChart_yAxis=[];//縱坐標

                     var TOTAL_RATE_Obj={};
                     var TOTAL_TARGET_YIELD_Obj={};

                     var TOTAL_RATE_yAxis=[];//實際良率縱坐標數據
                     var TOTAL_TARGET_YIELD_yAxis=[];//目標良率縱坐標數據

                     var xAxis_Set =new Set();
                     var ERRC_DESCR_Set = new Set();

                     var ERRC_DESCR=[];//當前製程段和線別底下的所有defectcode

                     if(hourRate.data.data.length>0){
                         hourRate.data.data.forEach(item=>{
                            xAxis_Set.add(item['TIMEHOUR']);
                            if(item['ERRC_DESCR']){
                                ERRC_DESCR_Set.add(item['ERRC_DESCR']);
                            }
                            TOTAL_RATE_Obj[item['TIMEHOUR']]=item['TOTAL_RATE'];
                            TOTAL_TARGET_YIELD_Obj[item['TIMEHOUR']]=item['TOTAL_TARGET_YIELD'];
                         });
                         hourRateChart_xAxis=Array.from(xAxis_Set);//獲取橫坐標數據
                         ERRC_DESCR=Array.from(ERRC_DESCR_Set);//獲取所有defect數據
                     }else{
                        hourRateChart_xAxis=[];
                        hourRateChart_yAxis=[];
                     }
                     //整理得到實際良率縱坐標數據
                     if(Object.keys(TOTAL_RATE_Obj).length>0){
                        for(var item in TOTAL_RATE_Obj){
                            TOTAL_RATE_yAxis.push(TOTAL_RATE_Obj[item]);
                        }
                     }
                     //整理得到目標良率縱坐標數據
                     if(Object.keys(TOTAL_TARGET_YIELD_Obj).length>0){
                        for(var item in TOTAL_RATE_Obj){
                            TOTAL_TARGET_YIELD_yAxis.push(TOTAL_TARGET_YIELD_Obj[item]);
                        }
                     }
                    

                     if(ERRC_DESCR.length>0){
                        ERRC_DESCR.forEach((ERRC,ERRC_index)=>{
                            var ERRC_obj={};
                            var ERRC_Array=[];
                            var defect_Array=[];
                            hourRate.data.data.forEach((RateValue,Rate_index)=>{
                                if(ERRC==RateValue['ERRC_DESCR']){
                                    ERRC_Array.push({
                                        'TIMEHOUR':RateValue['TIMEHOUR'],
                                         'MAIN_QTY':RateValue['MAIN_QTY']
                                    });
                                }else{
                                    ERRC_Array.push({
                                        'TIMEHOUR':RateValue['TIMEHOUR'],
                                         'MAIN_QTY':0
                                    });
                                }
                            });

                            for(let item of ERRC_Array){
                                if(ERRC_obj[item['TIMEHOUR']]){
                                    if(item['MAIN_QTY']>=1){
                                        ERRC_obj[item['TIMEHOUR']]=item['MAIN_QTY'];
                                    }
                                }else{
                                    ERRC_obj[item['TIMEHOUR']]=item['MAIN_QTY'];
                                }
                            }
                            //得到堆疊圖中的數據
                            for(let item in ERRC_obj){
                                defect_Array.push(ERRC_obj[item]);
                            }


                            hourRateChart_yAxis.push({
                                name: ERRC,
                                type: 'bar',
                                stack: 'DefectCode',
                                yAxisIndex:1,
                                data: defect_Array,
                                label:{
                                    show:true,
                                    position:'inside',
                                    formatter:function(val){
                                        if(val.value==0){
                                            return ''
                                        }else{
                                            return val.value
                                        }
                                     }
                                }
                            });

                            ERRC_Array=[];
                            defect_Array=[];
                            ERRC_obj={};
                        });
                     }

                     this.Option['xAxis']['data']=hourRateChart_xAxis;
                     this.Option['series']=hourRateChart_yAxis;
                     this.Option['series'].push({
                            name: '實際良率',
                            type: 'line',
                            yAxisIndex:0,
                            data: TOTAL_RATE_yAxis,
                            itemStyle: {
                                color:'blue'
                            },
                            label:{
                                show:true,
                                position :'top',
                                formatter:function(params){
                                    return params.value+'%'
                                }
                            }
                        });
                        this.Option['series'].push({
                            name: '目標良率',
                            type: 'line',
                            yAxisIndex:0,
                            data: TOTAL_TARGET_YIELD_yAxis,
                            itemStyle: {
                                color:'red'
                            },
                            label:{
                                show:true,
                                position :'top',
                                formatter:function(params){
                                    return params.value+'%'
                                }
                            }
                        });
                     this.Option.legend={
                            data: ERRC_DESCR.concat(['目標良率','實際良率']),
                            type: 'scroll',//圖例橫向滾動
                            left:0,
                            width:'80%'
                        }
                     this.Option.yAxis= [
                         {
                                type:'value',
                                axisLabel:{
                                    formatter:'{value} %'
                                },
                                scale:true,
                                splitLine: {
                                    show: false//是否显示分隔线。默认数值轴显示，类目轴不显示。
                                }
                          },
                          {
                                type:'value',
                                scale:true,
                                splitLine: {
                                    show: false//是否显示分隔线。默认数值轴显示，类目轴不显示。
                                }
                          }
                      ]

                     var hourRateChart=echarts.init(this.$refs.hourRateChart);
                     hourRateChart.setOption(this.Option);


                     //下面代碼的主要目的是恢復option到初始狀態,提供分時產出圖標使用

                     this.Option.legend.data=['計劃','實際'];
                     this.Option.legend={
                             show:true,
                            data: ['計劃','實際'],
                            orient: 'horizontal',
                            left: 'left', 
                            top: 'top',
                            textStyle: {
                                color: '#000',
                                fontSize: '12'
                            }//設置圖例的字體顏色和大小
                        }

                        this.Option.yAxis= {
                            type:'value',
                            scale:true,
                            splitLine: {
                                show: false//是否显示分隔线。默认数值轴显示，类目轴不显示。
                            }
                        }
                        this.Option.series=[{
                            name: '實際',
                            type: 'bar',
                            barWidth:'70%',
                            barMaxWidth:70,
                            data: [],
                            label:{
                                show:true,
                                position :'top',
                                textStyle: {
                                    color: '#555'
                                },
                                formatter:function(val){
                                    if(val.value==0){
                                        return ''
                                    }else{
                                        return val.value
                                    }
                                }
                            }
                        },
                        {
                            name: '計劃',
                            type: 'line',
                            data: [],
                            itemStyle: {
                                color:'red'
                            },
                            label:{
                                show:true,
                                position :'top'
                            }
                        }
                        ];
                     
                },//獲取派工人力
                pgQty:function(process,line){
                    axios.get('HomePage.aspx',{params:{'flag':'pgQty','process':process,'line':line}}).then(response=>{
                        this.pgInfo=response.data.data[0];
                    });
                },
                //加載線別
                loadLine:function(process){
                    axios.get('HomePage.aspx',{params:{'flag':'line','process':process}}).then((response)=>{
                       this.line=response.data.data;
                    });
                },
                //刷新倒計時
                loadRefresTime:function(){
                    this.refreshTimeInfo= this.ifZero(parseInt(this.refreshTime / 60)) + "分" + this.ifZero(this.refreshTime % 60) + "秒后數據自动更新";
                    window.setTimeout(()=>{
                            this.refreshTime--;
                        if(this.refreshTime==0){
                            this.refreshTime=15*60;
                        }
                    },1000);
                },
                //製程值監聽事件
                proSelect:function(value){
                    this.loadLine(value);
                },
                ifZero:function(value){
                    if(value<10){
                        return "0"+value;
                    }else{
                        return value;
                    }
                },
                //查詢
                query: function () {
                    this.lineShow=this.$refs.process.value+"-"+this.$refs.line.value;
                    var process=this.$refs.process.value;
                    var line=this.$refs.line.value;
                    this.loadQueryData(process,line);
                }
            }
        });
        function loadData(){
            var process=app.$refs.process.value;
            console.log(app.refreshTimeInfo,typeof app.refreshTimeInfo);
        }

        window.setInterval(app.loadRefresTime,1000);
        window.setInterval(app.query,15*60*1000);


    </script>
</body>
</html>