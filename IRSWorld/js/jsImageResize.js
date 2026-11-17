var popopt1="toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=yes,resizable=no";
var popopt=popopt1+",height=650,width=800";
var gi,gd,gj=1,gn=20,gt=40;
function sli(ii,wd1,ht1,wd2,ht2)
{
	gi=ii;
	gd=1;
	swd=wd1;
	sht=ht1;
	lwd=wd2;
	lht=ht2;
	rs();
}

function ssi(ii,wd1,ht1,wd2,ht2)
{
	gd=-1;
	swd=wd1;
	sht=ht1;
	lwd=wd2;
	lht=ht2;
	rs();
}

function xy(x1,x2,gj)
{
	return (x2-x1)*gj/gn+x1;
}

function rs()
{
	gi.style.width=xy(swd,lwd,gj)+"px";
	gi.style.height=xy(sht,lht,gj)+"px";
	if(gd>0 && gn>gj)
	{
		gj++;setTimeout('rs()',gt);
	}
	else if	(0>gd && gj>0)
	{
		gj--;setTimeout('rs()',gt);
	}
}
