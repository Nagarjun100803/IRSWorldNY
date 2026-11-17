//	written	by Tan Ling	Wee	on 2 Dec 2001
//	last updated 20 June 2003
//	email :	fuushikaden@yahoo.com
/******************************************************************
* Adapted & Modified by: Carlos H Raymundo for IRSWorld 10/26/2009
* - fire on change event when a date is selected.
* - detect the position of the popup relative to the body width.
* - break loop when offsetParent is null. (line 59 and 599) during calc pos.
* - Added optional fourth parameter in the call to popUpCalendar to let the
*   calendar scroll with its container.
* - solve year bug with IE10.
* - resolve image directory when parent several directory deeper
*******************************************************************/
	var	fixedX = -1;			// x position (-1 if to appear below control)
	var	fixedY = -1;		    // y position (-1 if to appear below control)
	var startAt = 1;			// 0 - sunday ; 1 - monday
	var showWeekNumber = 1;	    // 0 - don't show; 1 - show
	var showToday = 1;		    // 0 - don't show; 1 - show
	var imgDir = "../images/calendar/";	// directory for images ... e.g. var imgDir="/img/"

	var gotoString = "Go To Current Month";
	var todayString = "Today is";
	var weekString = "Wk";
	var scrollLeftMessage = "Click to scroll to previous month. Hold mouse button to scroll automatically.";
	var scrollRightMessage = "Click to scroll to next month. Hold mouse button to scroll automatically.";
	var selectMonthMessage = "Click to select a month.";
	var selectYearMessage = "Click to select a year.";
	var selectDateMessage = "Select [date] as date."; // do not replace [date], it will be replaced by date.

	var	crossobj, crossMonthObj, crossYearObj, monthSelected, yearSelected, dateSelected, omonthSelected, oyearSelected, odateSelected, monthConstructed, yearConstructed, intervalID1, intervalID2, timeoutID1, timeoutID2, ctlToPlaceValue, ctlNow, dateFormat, nStartingYear;

	var	bPageLoaded=false;
	var	ie=document.all;
	var	dom=document.getElementById;

	var	ns4=document.layers;
	var	today =	new	Date();
	var	dateNow	 = today.getDate();
	var	monthNow = today.getMonth();
	var	yearNow	 = today.getYear();
	var	imgsrc = new Array("drop1.gif","drop2.gif","left1.gif","left2.gif","right1.gif","right2.gif","close.gif");
	var	img	= new Array();

	var bShow = false;

    /* hides <select> and <applet> objects (for IE only) */
    function hideElement( elmID, overDiv )
    {
      if( ie )
      {
        var etags = document.getElementsByTagName(elmID);
		//chr 01/31/2012
        //for( i = 0; i < document.all.tags( elmID ).length; i++ )
        for (i=0; i < etags.length; i++)
        {
		  //chr 01/31/2012
          //obj = document.all.tags( elmID )[i];
          obj = etags[i];
          if( !obj || !obj.offsetParent )
          {
            continue;
          }

          // Find the element's offsetTop and offsetLeft relative to the BODY tag.
          objLeft   = obj.offsetLeft;
          objTop    = obj.offsetTop;
          objParent = obj.offsetParent;

          // changes by carlos h. raymundo - compute positioning
          while( objParent.tagName.toUpperCase() != "BODY" | objParent.tagName.toUpperCase() != "HTML" )
          {
            objLeft  += objParent.offsetLeft;
            objTop   += objParent.offsetTop;
            objParent = objParent.offsetParent;
            if (objParent == null) break; // by chr
          }
          // end changes

          objHeight = obj.offsetHeight;
          objWidth = obj.offsetWidth;

          if(( overDiv.offsetLeft + overDiv.offsetWidth ) <= objLeft );
          else if(( overDiv.offsetTop + overDiv.offsetHeight ) <= objTop );
          //commented by carlos raymundo
          //else if( overDiv.offsetTop >= ( objTop + objHeight ));
          //apply changes below:
          /* CHANGE by Charlie Roche for nested TDs*/
          else if(overDiv.offsetTop >= (objTop + objHeight + obj.height));
          /* END CHANGE */
          else if( overDiv.offsetLeft >= ( objLeft + objWidth ));
          else
          {
            obj.style.visibility = "hidden";
          }
        }
      }
    }

    /*
    * unhides <select> and <applet> objects (for IE only)
    */
    function showElement( elmID )
    {
      if( ie )
      {
        var etags = document.getElementsByTagName(elmID);
		//chr 01/31/2012
        //for( i = 0; i < document.all.tags( elmID ).length; i++ )
        for(i=0; i< etags.length; i++)
        {
		  //chr 01/31/2012
          //obj = document.all.tags( elmID )[i];
          obj = etags[i];
          if( !obj || !obj.offsetParent )
          {
            continue;
          }

          obj.style.visibility = "";
        }
      }
    }

	function HolidayRec (d, m, y, desc)
	{
		this.d = d;
		this.m = m;
		this.y = y;
		this.desc = desc;
	}

	var HolidaysCounter = 0;
	var Holidays = new Array();

	function addHoliday (d, m, y, desc)
	{
		Holidays[HolidaysCounter++] = new HolidayRec ( d, m, y, desc );
	}

	if (dom)
	{
		for	(i=0;i<imgsrc.length;i++)
		{
			img[i] = new Image;
			img[i].src = imgDir + imgsrc[i];
		}
		//chr 01/31/2012 remove all table width attribute and put it in style
		//document.write ("<div onclick='bShow=true' id='calendar' style='z-index:+999;position:absolute;visibility:hidden;'><table style='width:" + ((showWeekNumber==1)?250:220) + "px;font-family:arial;font-size:11px;border-width:1px;border-style:solid;border-color:#a0a0a0;font-family:arial; font-size:11px}' bgcolor='#ffffff'><tr bgcolor='#0000aa'><td><table style='width:"+((showWeekNumber==1)?248:218)+"px;'><tr><td style='width:80%;padding:2px;font-family:arial; font-size:11px;'><font color='#ffffff'><B><span id='caption'></span></B></font></td><td style='width:20%;text-align:right;' align=right><a href='javascript:hideCalendar()'><IMG SRC='"+imgDir+"close.gif' style='width: 15px; height:13px; border: 0' WIDTH='15' HEIGHT='13' BORDER='0' ALT='Close the Calendar'></a></td></tr></table></td></tr><tr><td style='padding:5px' bgcolor=#ffffff><span id='content'></span></td></tr>");
		document.write ("<div onclick='bShow=true' id='calendar' style='z-index:+999;position:absolute;visibility:hidden;'><table style='width:" + ((showWeekNumber==1)?250:220) + "px;font-family:arial;font-size:11px;border-width:1px;border-style:solid;border-color:#a0a0a0;font-family:arial; font-size:11px}' bgcolor='#ffffff'><tr bgcolor='#0000aa'><td><table style='width:100%;'><tr><td style='width:80%;padding:2px;font-family:arial; font-size:11px;'><font color='#ffffff'><B><span id='caption'></span></B></font></td><td style='width:20%;text-align:right;' align=right><a href='javascript:hideCalendar()'><IMG SRC='"+imgDir+"close.gif' style='width: 15px; height:13px; border: 0' WIDTH='15' HEIGHT='13' BORDER='0' ALT='Close the Calendar'></a></td></tr></table></td></tr><tr><td style='padding:5px' bgcolor=#ffffff><span id='content'></span></td></tr>");
		if (showToday==1)
		{
			document.write ("<tr bgcolor=#f0f0f0><td style='padding:5px' align=center><span id='lblToday'></span></td></tr>");
		}

		document.write ("</table></div><div id='selectMonth' style='z-index:+999;position:absolute;visibility:hidden;'></div><div id='selectYear' style='z-index:+999;position:absolute;visibility:hidden;'></div>");
	}

	var	monthName =	new	Array("January","February","March","April","May","June","July","August","September","October","November","December")
	var	monthName2 = new Array("JAN","FEB","MAR","APR","MAY","JUN","JUL","AUG","SEP","OCT","NOV","DEC")
	if (startAt==0)
	{
		dayName = new Array	("Sun","Mon","Tue","Wed","Thu","Fri","Sat");
	}
	else
	{
		dayName = new Array	("Mon","Tue","Wed","Thu","Fri","Sat","Sun");
	}
	var	styleAnchor="text-decoration:none;color:black;";
	var	styleLightBorder="border-style:solid;border-width:1px;border-color:#a0a0a0;";

	function swapImage(srcImg, destImg){
		if (ie)
		{
		    document.getElementById(srcImg).setAttribute("src",imgDir + destImg);
		}
	}

	function resolve_imgdir()
	{
	    if (window.location.pathname.toLowerCase().indexOf('reports/pages/report_holder') > -1)
	    {
	        imgDir = '../' + imgDir;
	    }
	}

	function init()
	{
		if (!ns4)
		{
		    //chr image dir path resolver
		    resolve_imgdir();

			if (!ie) { yearNow += 1900	};

			crossobj=(dom)?document.getElementById("calendar").style : ie? document.all.calendar : document.calendar;
			hideCalendar();

			crossMonthObj=(dom)?document.getElementById("selectMonth").style : ie? document.all.selectMonth	: document.selectMonth;

			crossYearObj=(dom)?document.getElementById("selectYear").style : ie? document.all.selectYear : document.selectYear

			monthConstructed=false;
			yearConstructed=false;

			if (showToday==1)
			{
				document.getElementById("lblToday").innerHTML =	todayString + " <a onmousemove='window.status=\""+gotoString+"\"' onmouseout='window.status=\"\"' title='"+gotoString+"' style='"+styleAnchor+"' href='javascript:monthSelected=monthNow;yearSelected=yearNow;constructCalendar();'>"+dayName[(today.getDay()-startAt==-1)?6:(today.getDay()-startAt)]+", " + dateNow + " " + monthName[monthNow].substring(0,3)	+ "	" +	yearNow	+ "</a>";
			}

			sHTML1="<span id='spanLeft'	style='width:10px; height:11px; border-style:solid;border-width:1;border-color:#3366FF;cursor:pointer' onmouseover='swapImage(\"changeLeft\",\"left2.gif\");this.style.borderColor=\"#88AAFF\";window.status=\""+scrollLeftMessage+"\"' onclick='javascript:decMonth()' onmouseout='clearInterval(intervalID1);swapImage(\"changeLeft\",\"left1.gif\");this.style.borderColor=\"#3366FF\";window.status=\"\"' onmousedown='clearTimeout(timeoutID1);timeoutID1=setTimeout(\"StartDecMonth()\",500)'	onmouseup='clearTimeout(timeoutID1);clearInterval(intervalID1)'>&nbsp<IMG id='changeLeft' SRC='"+imgDir+"left1.gif' width=10 height=11 BORDER=0>&nbsp</span>&nbsp;";
			sHTML1+="<span id='spanRight' style='width:10px; height:11px; border-style:solid;border-width:1;border-color:#3366FF;cursor:pointer'	onmouseover='swapImage(\"changeRight\",\"right2.gif\");this.style.borderColor=\"#88AAFF\";window.status=\""+scrollRightMessage+"\"' onmouseout='clearInterval(intervalID1);swapImage(\"changeRight\",\"right1.gif\");this.style.borderColor=\"#3366FF\";window.status=\"\"' onclick='incMonth()' onmousedown='clearTimeout(timeoutID1);timeoutID1=setTimeout(\"StartIncMonth()\",500)'	onmouseup='clearTimeout(timeoutID1);clearInterval(intervalID1)'>&nbsp<IMG id='changeRight' SRC='"+imgDir+"right1.gif'	width=10 height=11 BORDER=0>&nbsp</span>&nbsp";
			sHTML1+="<span id='spanMonth' style='border-style:solid;border-width:1;border-color:#3366FF;cursor:pointer'	onmouseover='swapImage(\"changeMonth\",\"drop2.gif\");this.style.borderColor=\"#88AAFF\";window.status=\""+selectMonthMessage+"\"' onmouseout='swapImage(\"changeMonth\",\"drop1.gif\");this.style.borderColor=\"#3366FF\";window.status=\"\"' onclick='popUpMonth()'></span>&nbsp;"
			sHTML1+="<span id='spanYear' style='border-style:solid;border-width:1;border-color:#3366FF;cursor:pointer' onmouseover='swapImage(\"changeYear\",\"drop2.gif\");this.style.borderColor=\"#88AAFF\";window.status=\""+selectYearMessage+"\"'	onmouseout='swapImage(\"changeYear\",\"drop1.gif\");this.style.borderColor=\"#3366FF\";window.status=\"\"'	onclick='popUpYear()'></span>&nbsp;"

			document.getElementById("caption").innerHTML  =	sHTML1;

			bPageLoaded=true;
		}
	}

	function hideCalendar()	{
		if (crossobj) crossobj.visibility="hidden";
		if (crossMonthObj != null){crossMonthObj.visibility="hidden"};
		if (crossYearObj !=	null){crossYearObj.visibility="hidden"};

		// added by carlos raymundo
		if (document.detachEvent)
		{
		    document.detachEvent('onclick', hidecal2);
		    document.detachEvent('onkeypress', hidecal1);
		}
		else
		{
		    document.removeEventListener('onclick', hidecal2, false);
		    document.removeEventListener('onkeypress', hidecal1, false);
		}
		// end addition

	    showElement( 'SELECT' );
		showElement( 'APPLET' );
	}

	function padZero(num) {
		return (num	< 10)? '0' + num : num ;
	}

	function constructDate(d,m,y)
	{
		sTmp = dateFormat;
		sTmp = sTmp.replace	("dd","<e>");
		sTmp = sTmp.replace	("d","<d>");
		sTmp = sTmp.replace	("<e>",padZero(d));
		sTmp = sTmp.replace	("<d>",d);
		sTmp = sTmp.replace	("mmmm","<p>");
		sTmp = sTmp.replace	("mmm","<o>");
		sTmp = sTmp.replace	("mm","<n>");
		sTmp = sTmp.replace	("m","<m>");
		sTmp = sTmp.replace	("<m>",m+1);
		sTmp = sTmp.replace	("<n>",padZero(m+1));
		sTmp = sTmp.replace	("<o>",monthName[m]);
		sTmp = sTmp.replace	("<p>",monthName2[m]);
		sTmp = sTmp.replace	("yyyy",y);
		return sTmp.replace ("yy",padZero(y%100));
	}

	function closeCalendar() {
		hideCalendar();
		ctlToPlaceValue.value =	constructDate(dateSelected,monthSelected,yearSelected);
		//Commented my previous modification to implement text on change by carlos raymundo - ie only
		//ctlToPlaceValue.onchange(); not working, replaced with codes below.
		if (ie)
		{
		    ctlToPlaceValue.fireEvent('onchange');
        }
        else
        {
            var evt = document.createEvent('HTMLEvents');
            evt.initEvent('change', false, false);
            ctlToPlaceValue.dispatchEvent(evt);
        }
    }

	/*** Month Pulldown	***/

	function StartDecMonth()
	{
		intervalID1=setInterval("decMonth()",80);
	}

	function StartIncMonth()
	{
		intervalID1=setInterval("incMonth()",80);
	}

	function incMonth ()
	{
		monthSelected++;
		if (monthSelected>11)
		{
			monthSelected=0;
			yearSelected++;
		}
		constructCalendar()
	}

	function decMonth ()
	{
		monthSelected--;
		if (monthSelected<0)
		{
			monthSelected=11;
			yearSelected--;
		}
		constructCalendar();
	}

	function constructMonth()
	{
		popDownYear();
		if (!monthConstructed)
		{
			sHTML =	"";
			for	(i=0; i<12;	i++)
			{
				sName =	monthName[i];
				if (i==monthSelected){
					sName =	"<B>" +	sName +	"</B>";
				}
				sHTML += "<tr><td id='m" + i + "' onmouseover='this.style.backgroundColor=\"#FFCC99\"' onmouseout='this.style.backgroundColor=\"\"' style='cursor:pointer' onclick='monthConstructed=false;monthSelected=" + i + ";constructCalendar();popDownMonth();event.cancelBubble=true'>&nbsp;" + sName + "&nbsp;</td></tr>";
			}

			document.getElementById("selectMonth").innerHTML = "<table	style='width:70px; font-family:arial; font-size:11px; border-width:1; border-style:solid; border-color:#a0a0a0;' bgcolor='#FFFFDD' cellspacing=0 onmouseover='clearTimeout(timeoutID1)'	onmouseout='clearTimeout(timeoutID1);timeoutID1=setTimeout(\"popDownMonth()\",100);event.cancelBubble=true'>" +	sHTML +	"</table>";

			monthConstructed=true;
		}
	}

	function popUpMonth()
	{
		constructMonth();
		crossMonthObj.visibility = (dom||ie)? "visible"	: "show";
		// chr 01/31/2012 put px in assignment to top and left
		crossMonthObj.left = parseInt(crossobj.left) + 50 + "px";
		crossMonthObj.top =	parseInt(crossobj.top) + 26 + "px";

		hideElement( 'SELECT', document.getElementById("selectMonth") );
		hideElement( 'APPLET', document.getElementById("selectMonth") );
	}

	function popDownMonth()	{
		crossMonthObj.visibility= "hidden";
	}

	/*** Year Pulldown ***/

	function incYear() {
		for	(i=0; i<7; i++)
		{
			newYear	= (i+nStartingYear)+1;
			if (newYear==yearSelected)
			{
			    txtYear =	"&nbsp;<B>"	+ newYear +	"</B>&nbsp;"
			}
			else
			{
			    txtYear =	"&nbsp;" + newYear + "&nbsp;"
			}
			document.getElementById("y"+i).innerHTML = txtYear;
		}
		nStartingYear++;
		bShow=true;
	}

	function decYear()
	{
		for	(i=0; i<7; i++)
		{
			newYear	= (i+nStartingYear)-1;
			if (newYear==yearSelected)
			{
			    txtYear =	"&nbsp;<B>"	+ newYear +	"</B>&nbsp;";
			}
			else
			{
			    txtYear =	"&nbsp;" + newYear + "&nbsp;";
			}
			document.getElementById("y"+i).innerHTML = txtYear;
		}
		nStartingYear --;
		bShow=true;
	}

	function selectYear(nYear)
	{
		yearSelected=parseInt(nYear+nStartingYear);
		yearConstructed=false;
		constructCalendar();
		popDownYear();
	}

	function constructYear()
	{
		popDownMonth();
		sHTML =	"";
		if (!yearConstructed)
		{

			sHTML =	"<tr><td align='center'	onmouseover='this.style.backgroundColor=\"#FFCC99\"' onmouseout='clearInterval(intervalID1);this.style.backgroundColor=\"\"' style='cursor:pointer'	onmousedown='clearInterval(intervalID1);intervalID1=setInterval(\"decYear()\",30)' onmouseup='clearInterval(intervalID1)'>-</td></tr>";

			j =	0;
			nStartingYear =	yearSelected-3;
			for	(i=(yearSelected-3); i<=(yearSelected+3); i++) {
				sName =	i;
				if (i==yearSelected){
					sName =	"<B>" +	sName +	"</B>"
				}

				sHTML += "<tr><td id='y" + j + "' onmouseover='this.style.backgroundColor=\"#FFCC99\"' onmouseout='this.style.backgroundColor=\"\"' style='cursor:pointer' onclick='selectYear("+j+");event.cancelBubble=true'>&nbsp;" + sName + "&nbsp;</td></tr>";
				j ++;
			}

			sHTML += "<tr><td align='center' onmouseover='this.style.backgroundColor=\"#FFCC99\"' onmouseout='clearInterval(intervalID2);this.style.backgroundColor=\"\"' style='cursor:pointer' onmousedown='clearInterval(intervalID2);intervalID2=setInterval(\"incYear()\",30)'	onmouseup='clearInterval(intervalID2)'>+</td></tr>";

			document.getElementById("selectYear").innerHTML	= "<table style='width:44px;font-family:arial; font-size:11px; border-width:1; border-style:solid; border-color:#a0a0a0;'	bgcolor='#FFFFDD' onmouseover='clearTimeout(timeoutID2)' onmouseout='clearTimeout(timeoutID2);timeoutID2=setTimeout(\"popDownYear()\",100)' cellspacing=0>"	+ sHTML	+ "</table>";

			yearConstructed	= true;
		}
	}

	function popDownYear() {
		clearInterval(intervalID1);
		clearTimeout(timeoutID1);
		clearInterval(intervalID2);
		clearTimeout(timeoutID2);
		crossYearObj.visibility = "hidden";
	}

	function popUpYear()
	{
		var	leftOffset;

		constructYear();
		crossYearObj.visibility	= (dom||ie)? "visible" : "show";
		leftOffset = parseInt(crossobj.left) + document.getElementById("spanYear").offsetLeft;
		if (ie)
		{
			leftOffset += 6;
		}
		// chr 01/31/2012 put px in assignment to top and left
		crossYearObj.left =	leftOffset + 'px';
		crossYearObj.top = parseInt(crossobj.top) +	26 + 'px';
	}

	/*** calendar ***/
   function WeekNbr(n) {
      // Algorithm used:
      // From Klaus Tondering's Calendar document (The Authority/Guru)
      // hhtp://www.tondering.dk/claus/calendar.html
      // a = (14-month) / 12
      // y = year + 4800 - a
      // m = month + 12a - 3
      // J = day + (153m + 2) / 5 + 365y + y / 4 - y / 100 + y / 400 - 32045
      // d4 = (J + 31741 - (J mod 7)) mod 146097 mod 36524 mod 1461
      // L = d4 / 1460
      // d1 = ((d4 - L) mod 365) + L
      // WeekNumber = d1 / 7 + 1

      year = n.getFullYear();
      month = n.getMonth() + 1;
      if (startAt == 0) {
         day = n.getDate() + 1;
      }
      else {
         day = n.getDate();
      }

      a = Math.floor((14-month) / 12);
      y = year + 4800 - a;
      m = month + 12 * a - 3;
      b = Math.floor(y/4) - Math.floor(y/100) + Math.floor(y/400);
      J = day + Math.floor((153 * m + 2) / 5) + 365 * y + b - 32045;
      d4 = (((J + 31741 - (J % 7)) % 146097) % 36524) % 1461;
      L = Math.floor(d4 / 1460);
      d1 = ((d4 - L) % 365) + L;
      week = Math.floor(d1/7) + 1;

      return week;
   }

	function constructCalendar ()
	{
		var aNumDays = Array (31,0,31,30,31,30,31,31,30,31,30,31);

		var dateMessage;
		var	startDate =	new	Date (yearSelected,monthSelected,1);
		var endDate;

		if (monthSelected==1)
		{
			endDate	= new Date (yearSelected,monthSelected+1,1);
			endDate	= new Date (endDate	- (24*60*60*1000));
			numDaysInMonth = endDate.getDate();
		}
		else
		{
			numDaysInMonth = aNumDays[monthSelected];
		}

		datePointer	= 0;
		dayPointer = startDate.getDay() - startAt;

		if (dayPointer<0)
		{
			dayPointer = 6;
		}

		sHTML =	"<table	 border=0 style='font-family:verdana;font-size:10px;'><tr>";

		if (showWeekNumber==1)
		{
			sHTML += "<td style='width:27px'><b>" + weekString + "</b></td><td rowspan=7 bgcolor='#d0d0d0' style='padding:0px'><img src='"+imgDir+"divider.gif' style='width:1px;' width=1></td>";
		}

		for	(i=0; i<7; i++)
		{
			sHTML += "<td style='width:27px' align='right'><B>"+ dayName[i]+"</B></td>";
		}
		sHTML +="</tr><tr>"

		if (showWeekNumber==1)
		{
			sHTML += "<td align=right>" + WeekNbr(startDate) + "&nbsp;</td>";
		}

		for	( var i=1; i<=dayPointer;i++ )
		{
			sHTML += "<td>&nbsp;</td>";
		}

		for	( datePointer=1; datePointer<=numDaysInMonth; datePointer++ )
		{
			dayPointer++;
			sHTML += "<td align=right>";
			sStyle=styleAnchor;
			if ((datePointer==odateSelected) &&	(monthSelected==omonthSelected)	&& (yearSelected==oyearSelected))
			{
			    sStyle += styleLightBorder;
			}

			sHint = "";
			for (k=0;k<HolidaysCounter;k++)
			{
				if ((parseInt(Holidays[k].d)==datePointer)&&(parseInt(Holidays[k].m)==(monthSelected+1)))
				{
					if ((parseInt(Holidays[k].y)==0)||((parseInt(Holidays[k].y)==yearSelected)&&(parseInt(Holidays[k].y)!=0)))
					{
						sStyle+="background-color:#FFDDDD;";
						sHint+=sHint==""?Holidays[k].desc:"\n"+Holidays[k].desc;
					}
				}
			}

			var regexp= /\"/g;
			sHint=sHint.replace(regexp,"&quot;");

			dateMessage = "onmousemove='window.status=\""+selectDateMessage.replace("[date]",constructDate(datePointer,monthSelected,yearSelected))+"\"' onmouseout='window.status=\"\"' ";

			if ((datePointer==dateNow)&&(monthSelected==monthNow)&&(yearSelected==yearNow))
			{
			    sHTML += "<b><a "+dateMessage+" title=\"" + sHint + "\" style='"+sStyle+"' href='javascript:dateSelected="+datePointer+";closeCalendar();'><font color=#ff0000>&nbsp;" + datePointer + "</font>&nbsp;</a></b>";
			}
			else if	(dayPointer % 7 == (startAt * -1)+1)
			{
			    sHTML += "<a "+dateMessage+" title=\"" + sHint + "\" style='"+sStyle+"' href='javascript:dateSelected="+datePointer + ";closeCalendar();'>&nbsp;<font color=#909090>" + datePointer + "</font>&nbsp;</a>";
			}
			else
			{
			    sHTML += "<a "+dateMessage+" title=\"" + sHint + "\" style='"+sStyle+"' href='javascript:dateSelected="+datePointer + ";closeCalendar();'>&nbsp;" + datePointer + "&nbsp;</a>";
		    }

			sHTML += "";
			if ((dayPointer+startAt) % 7 == startAt)
			{
				sHTML += "</tr><tr>";
				if ((showWeekNumber==1)&&(datePointer<numDaysInMonth))
				{
					sHTML += "<td align=right>" + (WeekNbr(new Date(yearSelected,monthSelected,datePointer+1))) + "&nbsp;</td>";
				}
			}
		}

		document.getElementById("content").innerHTML   = sHTML;
		document.getElementById("spanMonth").innerHTML = "&nbsp;" +	monthName[monthSelected] + "&nbsp;<IMG id='changeMonth' SRC='"+imgDir+"drop1.gif' WIDTH='12' HEIGHT='10' BORDER=0 style='width:12px; height:10px'>";
		document.getElementById("spanYear").innerHTML =	"&nbsp;" + yearSelected	+ "&nbsp;<IMG id='changeYear' SRC='"+imgDir+"drop1.gif' WIDTH='12' HEIGHT='10' BORDER=0 style='width:12px; height:10px'>";
	}

	function popUpCalendar(ctl,	ctl2, format, ctl_relto)
	{
		var	leftpos=0;
		var	toppos=0;

		document.onkeypress = hidecal1; // added by chr to isolate event
		document.onclick = hidecal2;    // added by chr to isolate event

		if (bPageLoaded)
		{
			if ( crossobj.visibility ==	"hidden" ) {
				ctlToPlaceValue	= ctl2;
				dateFormat=format;

				formatChar = " ";
				aFormat	= dateFormat.split(formatChar);
				if (aFormat.length<3)
				{
					formatChar = "/";
					aFormat	= dateFormat.split(formatChar);
					if (aFormat.length<2) // modified by chr to support mm/yyyy format (used to be < 3).
					{
						formatChar = ".";
						aFormat	= dateFormat.split(formatChar);
						if (aFormat.length<3)
						{
							formatChar = "-";
							aFormat	= dateFormat.split(formatChar);
							if (aFormat.length<3)
							{
								// invalid date	format
								formatChar="";
							}
						}
					}
				}

				tokensChanged =	0;
				if ( formatChar	!= "" )
				{
					// use user's date
					aData =	ctl2.value.split(formatChar);
					// change for irsworld by chr ** requires jsCommons.js
					if (aData.length == 1 && aData[0].length == 0)
					{
					    try
					    {
					        if (top.document.getElementById('hdr_date'))
					        {
					            aData = getText(top.document.getElementById('hdr_date')).split(' ')[0].split('/');
					        }
					        if (aData.length == 1 && trim(aData[0]).length == 0)
					        {
					            aData = dateToString(new Date()).split(' ')[0].split('/');
					        }
					    }
					    catch (e) {}
					}
					//for	(i=0;i<3;i++)
                    // end of change for irsworld by chr
                    var k = 0;
					for	(i=0;i<aFormat.length;i++)
					{
						if ((aFormat[i]=="d") || (aFormat[i]=="dd"))
						{
							dateSelected = parseInt(aData[k], 10);
							tokensChanged++;
						}
						else if	((aFormat[i]=="m") || (aFormat[i]=="mm"))
						{
							monthSelected =	parseInt(aData[k], 10) - 1;
							tokensChanged++;
						}
						else if	(aFormat[i]=="yyyy")
						{
						    if (aData[k].length < 4)
						    {
						        i--;
						    }
						    else
						    {
							    yearSelected = parseInt(aData[k], 10);
							    tokensChanged++;
							}
						}
						else if	(aFormat[i]=="mmm")
						{
							for	(j=0; j<12;	j++)
							{
								if (aData[k]==monthName[j])
								{
									monthSelected=j;
									tokensChanged++;
								}
							}
						}
						else if	(aFormat[i]=="mmmm")
						{
							for	(j=0; j<12;	j++)
							{
								if (aData[k]==monthName2[j])
								{
									monthSelected=j;
									tokensChanged++;
								}
							}
						}
						k++;
					}
				}

				if (aFormat[0] == 'mm' && aFormat[1] == 'yyyy')
				{
				    dateSelected = 1;
				    if (tokensChanged == 2) tokensChanged++;
				}

				if ((tokensChanged!=3) || isNaN(dateSelected) || isNaN(monthSelected) || isNaN(yearSelected))
				{
					dateSelected = dateNow;
					monthSelected =	monthNow;
					yearSelected = yearNow;
				}

				odateSelected = dateSelected;
				omonthSelected = monthSelected;
				oyearSelected = yearSelected;

				aTag = ctl;
				var sTag = aTag;
				do {
					aTag = aTag.offsetParent;
					if (aTag == null)
					{
					    aTag = sTag;
					    break;
					}
					leftpos	+= aTag.offsetLeft;
					toppos += aTag.offsetTop;
					sTag = aTag;
				} while(aTag.tagName!="BODY" | aTag.tagName != "HTML");

				// dynamically compute the position of the popup. (chr)
				if (aTag != null) //&& aTag.tagName == "BODY")
				{
				    try
				    {
				        var bodiWidth = aTag.offsetWidth;
				        var bodiHeight = aTag.offsetHeight;
				        leftpos = ctl.offsetLeft + leftpos;
				        if ( (bodiWidth - (leftpos + ctl.offsetWidth) ) < 323)
				        {
				            leftpos = leftpos + ctl.offsetWidth - (323 + 3);
				        }
				        var stoppos = toppos;
				        if ( (bodiHeight - (toppos + ctl.offsetHeight)) < 166)
				        {
				            toppos = toppos - ctl.offsetHeight - (166 + 3);
				        }
				        if (toppos < 0) toppos = stoppos;
				    }
				    catch (e) {}
				}

				var oristop = 0; // tracks the scrollTop

				if (ctl_relto != null && (ctl_relto.style.overflow != null && ctl_relto.style.overflow.length > 0))
				{
				    oristop = ctl_relto.scrollTop;
				    toppos -= oristop;
				}

				//modified by chr for firefox. added 'px' in left and top.
				crossobj.left =	(fixedX==-1 ? leftpos :	fixedX) + 'px';
				crossobj.top = (fixedY==-1 ?	 ctl.offsetTop + toppos + ctl.offsetHeight + 2 : fixedY) + 'px';
				constructCalendar(1, monthSelected, yearSelected);

				crossobj.visibility = (dom||ie) ? "visible" : "show";

				hideElement( 'SELECT', document.getElementById("calendar") );
				hideElement( 'APPLET', document.getElementById("calendar") );

				bShow = true;

				//chr this checks the 4th parameter if scrolling and let the calendar move with the container
				if (ctl_relto != null && (ctl_relto.style.overflow != null && ctl_relto.style.overflow.length > 0))
				{
				    ctl_relto.onscroll = function()
				    {
				        var step = (ctl_relto.scrollTop < oristop) ? oristop - ctl_relto.scrollTop : ctl_relto.scrollTop - oristop;
				        var y = (ctl_relto.scrollTop < oristop) ? crossobj.posTop + step : crossobj.posTop - step;
                        crossobj.top = y + "px";
                        oristop = ctl_relto.scrollTop;
				    }
				}
				//end (chr)
			}
			else
			{
				hideCalendar();
				if (ctlNow!=ctl) {popUpCalendar(ctl, ctl2, format)};
			}
			ctlNow = ctl;
		}
	}

    //  commented by chr.
	//  document.onkeypress = function hidecal1 () {
	function hidecal1 () {
		if (event.keyCode==27)
		{
			hideCalendar();
		}
	}

	//  commented by chr
	//  document.onclick = function hidecal2 () {
	function hidecal2 ()
	{
		if (!bShow)
		{
			hideCalendar();
		}
		bShow = false;
	}

    /* Commented by carlos h. raymundo. not working in firefox.
	if(ie)
	{
		init();
	}
	else
	{
		window.onload=init;
	}
	*/

	init();
