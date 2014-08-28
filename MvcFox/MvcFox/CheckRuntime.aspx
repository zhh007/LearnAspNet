<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        .datagrid table
        {
            border-collapse: collapse;
            text-align: left;
            width: 100%;
        }
        .datagrid
        {
            font: normal 12px/150% Arial, Helvetica, sans-serif;
            background: #fff;
            overflow: hidden;
            border: 1px solid #006699;
            -webkit-border-radius: 3px;
            -moz-border-radius: 3px;
            border-radius: 3px;
        }
        .datagrid table td, .datagrid table th
        {
            padding: 3px 10px;
        }
        .datagrid table thead th
        {
            background: -webkit-gradient( linear, left top, left bottom, color-stop(0.05, #006699), color-stop(1, #00557F) );
            background: -moz-linear-gradient( center top, #006699 5%, #00557F 100% );
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#006699', endColorstr='#00557F');
            background-color: #006699;
            color: #ffffff;
            font-size: 15px;
            font-weight: bold;
            border-left: 1px solid #0070A8;
        }
        .datagrid table thead th:first-child
        {
            border: none;
        }
        .datagrid table tbody td
        {
            color: #000;
            /*border-left: 1px solid #E1EEF4;*/
            border: 1px solid #E1EEF4;
            font-size: 12px;
            font-weight: normal;
            height:20px;
        }
        .datagrid table tbody .alt td
        {
            background: #E1EEf4;
            color: #000;
            border: 1px solid #E1EEF4;
        }
        .datagrid table tbody td:first-child
        {
            border-left: none;
        }
        .datagrid table tbody tr:last-child td
        {
            border-bottom: none;
        }
        .datagrid table tfoot td div
        {
            border-top: 1px solid #006699;
            background: #E1EEf4;
        }
        .datagrid table tfoot td
        {
            padding: 0;
            font-size: 12px;
        }
        .datagrid table tfoot td div
        {
            padding: 2px;
        }
        .datagrid table tfoot td ul
        {
            margin: 0;
            padding: 0;
            list-style: none;
            text-align: right;
        }
        .datagrid table tfoot li
        {
            display: inline;
        }
        .datagrid table tfoot li a
        {
            text-decoration: none;
            display: inline-block;
            padding: 2px 8px;
            margin: 1px;
            color: #FFFFFF;
            border: 1px solid #006699;
            -webkit-border-radius: 3px;
            -moz-border-radius: 3px;
            border-radius: 3px;
            background: -webkit-gradient( linear, left top, left bottom, color-stop(0.05, #006699), color-stop(1, #00557F) );
            background: -moz-linear-gradient( center top, #006699 5%, #00557F 100% );
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#006699', endColorstr='#00557F');
            background-color: #006699;
        }
        .datagrid table tfoot ul.active, .datagrid table tfoot ul a:hover
        {
            text-decoration: none;
            border-color: #00557F;
            color: #FFFFFF;
            background: none;
            background-color: #006699;
        }
        div.dhtmlx_window_active, div.dhx_modal_cover_dv
        {
            position: fixed !important;
        }
        
        .tg_close
        {
            background: url('http://www.datatables.net/examples/resources/details_open.png') no-repeat left center;
            display:inline-block; padding-left: 24px; cursor:pointer
        }
        .tg_open
        {
            background: url('http://www.datatables.net/examples/resources/details_close.png') no-repeat left center;
            display:inline-block; padding-left: 24px; cursor:pointer
        }
        
        
        /* -- for main McTabs */

ul#maintabs
{
    height:30px; /* height #1 */
    text-align:left; 
    margin:0; 
    padding:0;
    padding-left:20px; 
    font-size:0;
    list-style-type:none;
}
        
ul#maintabs li
{
    display:inline;
    margin:0;padding:0;
    margin-right:1px;
}
        
ul#maintabs li a
{
    padding:0 10px;
    display:inline-block;    
    /*font:bold 12px Verdana;*/
    font-weight:600;
    font-size:15px;
    font-family: 'Open Sans Condensed',sans-serif;
    line-height:30px;/* height #1 */
    text-decoration: none;
    color:#888;
    border:2px solid transparent;
    border-bottom:none;
    background:white;
    outline:none;
    border-radius:5px 5px 0 0;
    position:relative;
}
        
ul#maintabs li a:link, ul#maintabs li a:visited
{
    color:#888;
}
        
ul#maintabs li a:hover
{
    border-color: transparent;
    background:white; /*color #2*/
    color:#333;
}
  
/*selected tab style */
ul#maintabs li.selected a
{
    color:#333;
    font-weight:bold;
    border-color:#CCC; /*color #1*/
    background:white; /*color #2*/
    z-index:3;
} 
        
/*selected tab style on hover */
ul#maintabs li.selected a:hover
{
    text-decoration:none;
    color:Black;
}

/* container of content panels */
div#maintabs-panel-container
{
    border:none;
    border-top:2px solid #CCC; /*color #1*/ 
    border-radius:0px;
    background-color:white; /*color #2*/
    position:relative;    
    padding:0px; margin:0px;
}

/* content panel */       
div#maintabs-panel-container > div
{
    padding:30px 0px;
    display: block;
    margin:0px;
}

#maintabs-panel-container div.ajaxLoading {background:transparent url(loading.gif) no-repeat center center; height:150px; font-size:0;padding:0; margin:0; }

/* -- end main McTabs */
        
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <ul id="maintabs" class="mctabs">
        <li class="selected"><a href="#view1">Overview</a></li>
        <li class=""><a href="#view2">HTML</a></li>
        <li class=""><a href="#view3">CSS</a></li>
        <li class=""><a href="#view4">Download</a></li>
        <li class=""><a href="#view5">Further Customization</a></li>
        <li class=""><a href="#view6">License</a></li>
    </ul>
    <div id="maintabs-panel-container" class="panel-container" style="height: auto;">
        <div id="view1" style="display: block; opacity: 1;">
            
        </div>
        <div id="view3" style="display: none; opacity: -0.09999999999999998;">
            
        </div>
        <div id="view4" style="display: none; opacity: -0.09999999999999998;">
            
        </div>
        <div id="view5" style="display: none; opacity: -0.09999999999999998;">
            
        </div>
        <div id="view6" style="display: none; opacity: -0.09999999999999998;">
        </div>    
    </div>

        <%= Version %>
        <%= GetLoadedAssemblies()%>
    </div>
    </form>
</body>
</html>
<script type="text/javascript">
    function taggleDetail(detailId, obj) {
        if(obj.className == "tg_close") {
            document.getElementById(detailId).style.display = "";
            obj.className = "tg_open";
        } else {
            document.getElementById(detailId).style.display = "none";
            obj.className = "tg_close";
        }
    }
</script>

<script runat="server">
    string Version;
    protected void Page_Load(object sender, EventArgs e)
    {
        Version = string.Format("v{0}.{1}.{2}.{3}", Environment.Version.Major, Environment.Version.Minor, Environment.Version.Build, Environment.Version.Revision);
    }

    private static int CompareAssemblyFullName(System.Reflection.Assembly x, System.Reflection.Assembly y)
    {
        return x.FullName.CompareTo(y.FullName);
    }

    class AssRefItem
    {
        public string MainAssemblyName { get; set; }
        public string RefAssemblyName { get; set; }
    }
    
    private string GetLoadedAssemblies()
    {
        System.Reflection.Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

        List<AssRefItem> list = new List<AssRefItem>();
        foreach (System.Reflection.Assembly ass in loadedAssemblies)
        {
            System.Reflection.AssemblyName[] assNames = ass.GetReferencedAssemblies();
            foreach (System.Reflection.AssemblyName assName in assNames)
            {
                AssRefItem refItem = new AssRefItem();
                refItem.MainAssemblyName = ass.FullName;
                refItem.RefAssemblyName = assName.FullName;
                list.Add(refItem);
            }
        }
        
        string ver = string.Format("v{0}.{1}.{2}", Environment.Version.Major, Environment.Version.Minor, Environment.Version.Build);
        
        StringBuilder sb = new StringBuilder();
        sb.Append("<div class='datagrid' style='width:1000px'>");
        sb.Append("<table>");
        sb.Append("<thead>");
        sb.Append("<th>#</th>");
        sb.Append("<th>FullName</th>");
        sb.Append("<th>In Global</th>");
        sb.Append("<th>Runtime Version</th>");
        sb.Append("</thead>");
        sb.Append("<tbody>");

        Array.Sort(loadedAssemblies, CompareAssemblyFullName);

        int index = 0;
        foreach (System.Reflection.Assembly ass in loadedAssemblies)
        {
            sb.AppendFormat("<tr id='a{1}' class='{0}'>", (index % 2 == 0) ? "" : "alt", index);
            sb.AppendFormat("<td><a class='tg_close' onclick='taggleDetail(\"a{1}_detail\", this)'>{0}</a></td>", index + 1, index);
            sb.AppendFormat("<td style='white-space:nowrap; word-break:keep-all;'>{0}</td>", ass.FullName);
            try
            {
                if (ass.GlobalAssemblyCache)
                {
                    sb.AppendFormat("<td style='font-weight:bold'>{0}</td>", ass.GlobalAssemblyCache);
                }
                else
                {
                    sb.AppendFormat("<td>{0}</td>", ass.GlobalAssemblyCache);
                }

                if (ass.ImageRuntimeVersion != ver)
                {
                    sb.AppendFormat("<td style='color:red'>{0}</td>", ass.ImageRuntimeVersion);
                }
                else
                {
                    sb.AppendFormat("<td>{0}</td>", ass.ImageRuntimeVersion);
                }
            }
            catch (Exception)
            {

            }
            sb.Append("</tr>");

            sb.AppendFormat("<tr id='a{0}_detail' style='display:none'>", index);
            sb.Append("<td></td>");
            sb.Append("<td colspan='3'>");
            
            sb.Append(GetDetailHtml(ass, list));

            sb.Append("</td>");
            sb.Append("</tr>");
            
            //sb.Append("<tr>");
            //try
            //{
            //    sb.AppendFormat("<td style='text-align:right' >{0}</td>", ass.CodeBase.Substring(8).Replace("/", "\\"));
            //    sb.Append("<td></td><td></td>");
            //}
            //catch (Exception)
            //{

            //}
            //sb.Append("</tr>");

            index++;
        }
        sb.Append("</tbody>");
        sb.Append("</table>");
        sb.Append("</div>");
        return sb.ToString();
    }

    private string GetDetailHtml(System.Reflection.Assembly ass, List<AssRefItem> refList)
    {
        StringBuilder sb = new StringBuilder();
        
        sb.Append("<div style='margin:5px'>");
        sb.Append("<table>");
        
        sb.Append("<tr>");
        sb.Append("<td>Referenced Assemblies</td>");
        sb.Append("<td>");
        foreach (AssRefItem item in refList)
        {
            if (item.MainAssemblyName == ass.FullName)
            {
                sb.AppendFormat("<div>{0}</div>", item.RefAssemblyName);
            }
        }
        sb.Append("</td>");
        sb.Append("</tr>");

        sb.Append("<tr>");
        sb.Append("<td>Referenced By Assemblies</td>");
        sb.Append("<td>");
        foreach (AssRefItem item in refList)
        {
            if (item.RefAssemblyName == ass.FullName)
            {
                sb.AppendFormat("<div>{0}</div>", item.MainAssemblyName);
            }
        }
        sb.Append("</td>");
        sb.Append("</tr>");
        
        
        sb.Append("</table>");
        sb.Append("</div>");        
        
        return sb.ToString();
    }
</script>
