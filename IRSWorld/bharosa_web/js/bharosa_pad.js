/*
 * Copyright (c) 2008 Oracle
 * All rights reserved.
 *
 * This software is the confidential and proprietary information of
 * Oracle ("Confidential Information").  You shall not
 * disclose such Confidential Information and shall use it only in
 * accordance with the terms of the license agreement you entered into
 * with Oracle.	
 */

var undefined;

T_KEYPAD="T_KEYPAD";
T_KEYPADNOIMG="T_KEYPADNOIMG";
T_PINPAD="T_PINPAD";
T_PINPADNOIMG="T_PINPADNOIMG";
T_QUIZPAD="T_QUIZPAD";
T_QUIZPADNOIMG="T_QUIZPADNOIMG";
T_TEXTPAD="T_TEXTPAD";
T_TEXTPADRESET="T_TEXTPADRESET";
T_TEXTPADNOIMG="T_TEXTPADNOIMG";
T_TEXTPADRESETNOIMG="T_TEXTPADRESETNOIMG";
T_CAPTIONPAD="T_CAPTIONPAD";
T_CAPTIONPADNOIMG="T_CAPTIONPADNOIMG";
T_CAPTIONCONFIRMPAD="T_CAPTIONCONFIRMPAD";
T_CAPTIONCONFIRMPADNOIMG="T_CAPTIONCONFIRMPADNOIMG";
T_QUESTIONPAD="T_QUESTIONPAD";
T_QUESTIONPADNOIMG="T_QUESTIONPADNOIMG";

var page_pads = new Array();

function Bharosa_Pad(initObj){

	this.name = "";
	this.type=T_KEYPAD;

	this.url = "";
	this.bImgLoc = "";
	this.capsOnImgLoc = "";
	this.capsShiftImgLoc = "";
	this.capsOnStyle = "";
	this.capsShiftStyle = "";

	this.map = "";
	this.keySet= new Object();
	this.captionText = "";

	this.adaComp = false;
	this.shiftDown=false;
	this.capsDown=false;
	this.lastEnterClick="";

	this.showDataDiv=true;
	this.dataDivStyle="";
	this.dataInputStyle="";
	this.confirmDivStyle="";
	this.confirmInputStyle="";
	this.maxLength=9;
	this.userInput="";

	this.ciph="";
	this.archive="";

	this.dataDiv;
	this.confirmDiv;
	this.keyDiv;
	this.keyBGDiv;
	this.backspaceDiv;
	this.closeDiv;
	this.dataField;
	this.confirmField;
	this.capsShiftDiv;
	this.capsOnDiv;

	this.left="0";
	this.top="0";
	this.height=0;
	this.width=0;

	this.imgAlt="Security Device Image";
	
	this.canDrag=true;
	this.canHide=true;
	this.startVisible=false;
	this.submitForm=true;

	this.mouseX=0;
	this.mouseY=0;

	this.mapStartX=8;
	this.mapStartY=38;

	this.isIE=true;
	this.isIE11 = false;
	this.isMac=false;
	this.isSafari2=false;
	this.isChrome=false;

	this.hasCaps=false;

  this.setFocus = function f(){
    if (this.type != T_TEXTPAD && this.type != T_TEXTPADNOIMG
				&& this.type != T_TEXTPADRESET && this.type != T_TEXTPADRESETNOIMG
				&& this.type != T_CAPTIONPAD && this.type != T_CAPTIONPADNOIMG
				&& this.type != T_QUESTIONPAD && this.type != T_QUESTIONPADNOIMG){

    } else {
      if (this.dataField != undefined){
        this.dataField.focus();
      }
    }
  }

  this.reset = function f(){
		this.ciph = "";
		if (this.dataField != undefined){
			if (this.type != T_TEXTPAD && this.type != T_TEXTPADNOIMG
				&& this.type != T_TEXTPADRESET && this.type != T_TEXTPADRESETNOIMG
				&& this.type != T_CAPTIONPAD && this.type != T_CAPTIONPADNOIMG
				&& this.type != T_QUESTIONPAD && this.type != T_QUESTIONPADNOIMG){
				this.dataField.value = ":";
        if (this.dataDiv != undefined){
            this.dataDiv.innerHTML = "";
        }
			}else{
				this.dataField.value = "";
				if (this.type != T_TEXTPADRESET && this.type != T_TEXTPADRESETNOIMG){
					if (this.confirmField != undefined){
						this.confirmField.value = "";
					}
				}
			}
		}
	};

	this.cycleShiftCaps = function f(){
		if (!this.capsDown && !this.shiftDown){
			this.shiftDown = !this.shiftDown;
		} else if (!this.capsDown && this.shiftDown){
			this.shiftDown = !this.shiftDown;
			this.capsDown = !this.capsDown;
		} else {
			this.capsDown = !this.capsDown;
		}
		this.showCaps();
	};

	this.toggleCaps = function f(){
		this.capsDown = !this.capsDown;
		this.showCaps();
	};

	this.toggleShift = function f(){
		this.shiftDown = !this.shiftDown;
		this.showCaps();
	};

	this.keyPress = function f(aKey){
		var displayText = "";
		if (this.dataDiv != undefined){
			displayText = this.dataDiv.innerHTML;
		}

		switch(aKey){
			case "CLOSE":
				this.hide();
				break;

			case "BSKEY":
				this.ciph = this.ciph.substring(0,this.ciph.length-1);
				var index = this.ciph.indexOf(':');
				var lastIndex = this.ciph.lastIndexOf(':');

				if (index == -1) {
					this.ciph="";
				} else {
					   this.ciph = this.ciph.substring(0,lastIndex);
				}

				if (displayText.length > 0) {
					displayText = displayText.substring(0,displayText.length-1);
				}

				this.ciph += ":";
				break;
			case "CLEAR":
				this.ciph = ":";
				displayText = "";
				break;

			case "ENTERKEY":
				this.clickEnter();
				break;

			case "CAPSLOCKKEY":
				this.cycleShiftCaps();
				break;

			default:
				if (displayText.length < this.maxLength){
					if(this.adaComp){
						if (this.shiftDown || this.capsDown){
							this.ciph+=(aKey+";:");

						}else {
							this.ciph+=(aKey+":");
						}
					}else{
						if (this.shiftDown || this.capsDown){
							this.ciph+=(this.mouseX +","+this.mouseY+";:");
						}else{
							this.ciph+=(this.mouseX+","+this.mouseY+":");
						}
					}

					if (this.ciph != "" && this.ciph != ':') {
							displayText += "*";
					} else {
							displayText = "";
					}
				}

				if(this.shiftDown){
					this.toggleShift();
				}
				break;
			}

			if (this.type == T_PINPAD || this.type == T_PINPADNOIMG
				|| this.type == T_KEYPAD || this.type == T_KEYPADNOIMG ){
				if (this.dataDiv != undefined){
					if (this.isMac){
						this.dataDiv.innerHTML = "";
					}
					this.dataDiv.innerHTML = displayText;
				}
			} else if (this.type == T_QUIZPAD || this.type == T_QUIZPADNOIMG){
				this.clickEnter();
			}

	};

	this.keyPressDbl = function f(aKey){
		if (this.isIE){
			this.keyPress(aKey);
		}
	};

	this.clickEnter = function f(){
		var padForm = this.dataField.form;

		this.setCiphValues();
		if (this.externalField != undefined){
			this.externalField.value = this.dataDiv.innerHTML;
		}

		var now = new Date();
		if ((this.lastEnterClick == "") || (now - this.lastEnterClick >= 3000)){
			this.lastEnterClick = now;
			if (this.customClickEnter() && padForm && this.submitForm){
				padForm.submit();
			}
		}
	};

	this.customClickEnter = function f(){return true};

	this.setCiphValues = function f(){
		if (this.dataField && this.ciph) {
			if (this.type != T_TEXTPAD && this.type != T_TEXTPADNOIMG
				&& this.type != T_TEXTPADRESET && this.type != T_TEXTPADRESETNOIMG
				&& this.type != T_CAPTIONPAD && this.type != T_CAPTIONPADNOIMG
				&& this.type != T_QUESTIONPAD && this.type != T_QUESTIONPADNOIMG){
				this.dataField.value = this.ciph;
			}
		}
	};

	this.showCaps = function f(){
		if (this.hasCaps){
			if (this.capsShiftDiv != undefined){
				if (this.shiftDown){
					this.capsShiftDiv.style.visibility="visible";
				}else{
					this.capsShiftDiv.style.visibility="hidden";
				}
			}

			if (this.capsOnDiv != undefined){
				if (this.capsDown){
					this.capsOnDiv.style.visibility="visible";
				}else{
					this.capsOnDiv.style.visibility="hidden";
				}
			}
		}
	};

	this.hide = function f(){
//		this.padDiv.style.visibility="hidden";
		this.padDiv.style.display="none";
		if (this.hasCaps){
			if (this.capsShiftDiv != undefined){
				this.capsShiftDiv.style.visibility="hidden";
			}
			if (this.capsOnDiv != undefined){
				this.capsOnDiv.style.visibility="hidden";
			}
		}
		if (this.showDataDiv){
			if (this.dataDiv != undefined){
				this.dataDiv.style.visibility="hidden";
			}
			if (this.type == T_TEXTPADRESET || this.type == T_TEXTPADRESETNOIMG){
				this.confirmDiv.style.visibility="hidden";
			}
		}
		if (!this.canDrag){
			this.padDiv.style.position="absolute";
		}
		if (this.type == T_PINPADNOIMG){
			if (this.keyDiv != undefined){
				this.keyDiv.style.visibility="hidden";
			}
			if (this.keyBGDiv != undefined){
				this.keyBGDiv.style.visibility="hidden";
			}
			if (this.backspaceDiv != undefined){
				this.backspaceDiv.style.visibility="hidden";
			}
		}
		if (this.type == T_PINPADNOIMG || this.type == T_TEXTPADNOIMG || this.type == T_TEXTPADRESETNOIMG || this.type == T_CAPTIONPADNOIMG || this.type == T_QUESTIONPADNOIMG){
			if (this.closeDiv != undefined){
				this.closeDiv.style.visibility="hidden";
			}
		}
	};

	this.show = function f(locObj){
		var locationObj = locObj;
		if (locationObj != undefined && this.canDrag){
			if (locationObj != null){
				var objLeft = locationObj.offsetLeft;
				var objTop = locationObj.offsetTop;

				while (locationObj.offsetParent.tagName != "BODY"){
					locationObj = locationObj.offsetParent;
					objLeft += locationObj.offsetLeft;
					objTop += locationObj.offsetTop;
				}

				if (objTop){
					this.top = objTop;
				}
				if (objLeft){
					this.left = objLeft;
				}
			}
		}else if(!this.canDrag){
			this.padDiv.style.position="relative";
		}

//		this.padDiv.style.visibility="visible";
		this.padDiv.style.display="";
		this.padDiv.style.top = this.top;
		this.padDiv.style.left = this.left;
		if (this.hasCaps){
			this.showCaps();
		}
		if (this.showDataDiv){
			if (this.dataDiv != undefined){
				this.dataDiv.style.visibility="visible";
			}
			if (this.type == T_TEXTPADRESET || this.type == T_TEXTPADRESETNOIMG){
				if (this.confirmDiv != undefined){
					this.confirmDiv.style.visibility="visible";
				}
			}
		}
		if (this.type == T_PINPADNOIMG){
			if (this.keyDiv != undefined){
				this.keyDiv.style.visibility="visible";
			}
			if (this.keyBGDiv != undefined){
				this.keyBGDiv.style.visibility="visible";
			}
			if (this.backspaceDiv != undefined){
				this.backspaceDiv.style.visibility="visible";
			}
		}

		if (this.type == T_PINPADNOIMG || this.type == T_TEXTPADNOIMG || this.type == T_TEXTPADRESETNOIMG || this.type == T_CAPTIONPADNOIMG || this.type == T_QUESTIONPADNOIMG){
			if (this.closeDiv != undefined){
				this.closeDiv.style.visibility="visible";
			}
		}

		for (var aPadName in page_pads){
			var aPad = page_pads[aPadName];
			if (aPad.name != this.name){
				aPad.hide();
			}
		}
	};

	this.trackMouse = function f(e){
		var tempX = 0;
		var tempY = 0;

		if (this.isIE){
			if (!this.isMac){
				if (this.padDiv.style.position == "absolute"){
					tempX = event.x - this.padDiv.offsetLeft + document.body.scrollLeft;
					tempY = event.y - this.padDiv.offsetTop + document.body.scrollTop;
				} 
				else if (this.isIE11)
				{
					tempX = parseInt(event.offsetX);
					tempY = parseInt(event.offsetY);
				}
				else {
					tempX = event.x;
					tempY = event.y;
				}
			} else {
				tempX = event.offsetX + document.body.scrollLeft + this.mapStartX;
				tempY = event.offsetY + document.body.scrollTop + this.mapStartY;
			}
		} else {
			if (e){
				tempX = e.pageX - (this.padDiv.offsetLeft);
				tempY = e.pageY - (this.padDiv.offsetTop);
				/*
				if (this.isSafari2 && this.padDiv.style.position == "relative"){
					tempX -= 8;
					tempY -= 8;
				}
				*/
			}
		}

		if (tempX < 0){tempX = 0;}
		if (tempY < 0){tempY = 0;}

		this.mouseX = tempX;
		this.mouseY = tempY;
		return true;
	};

	this.draw = function f(){

		document.write('<div id="' + this.name + 'TestBrowseDiv"></div>');

		this.isIE = document.getElementById(this.name + "TestBrowseDiv").all?true:false;
		if (!this.isIE) 
		{
		    this.isIE = (navigator.userAgent.indexOf("MSIE") > -1 | navigator.userAgent.indexOf("rv:") > -1);
		    if (this.isIE && navigator.userAgent.indexOf("Firefox") > -1)
		    {
		        this.isIE = false;
		    }
		}
		
        if (this.isIE)
        {
            if (navigator.userAgent.indexOf("rv:") > -1)
            {
                this.isIE11 = true;
            }
            else if (document.body.style.msTouchAction != undefined)
            {
                this.isIE11 = true;
            }
            else if (navigator.userAgent.indexOf("Trident/6") > -1 | navigator.userAgent.indexOf("Trident/7") > -1)
            {
                this.isIE11 = true;
            }
        }
        		
		this.isMac = (navigator.appVersion.indexOf("Mac") != -1);
		
		if (navigator.appVersion.indexOf("Safari") != -1){
			var versionRegExp = /Version\/(\d+)\.\d+\.\d+/;
			var regExpResult = navigator.appVersion.match(versionRegExp);
			if (navigator.userAgent.indexOf("Chrome") != -1)
			{
			    this.Chrome = true;
			}
			else if (!regExpResult || (2 >= regExpResult[1])){
				this.isSafari2 = true;
			}
		}
		
		if (this.type == T_KEYPAD){
			try{ KeyPad.draw(this); } catch (e){}
		}else if (this.type == T_KEYPADNOIMG){
			try{ KeyPadNoImg.draw(this); } catch (e){}
		}else if (this.type == T_PINPAD){
			try{ PinPad.draw(this); } catch (e){}
		}else if (this.type == T_PINPADNOIMG){
			try{ PinPadNoImg.draw(this); } catch (e){}
		}else if (this.type == T_QUIZPAD){
			try{ QuizPad.draw(this); } catch (e){}
		}else if (this.type == T_QUIZPADNOIMG){
			try{ QuizPadNoImg.draw(this); } catch (e){}
		}else if (this.type == T_TEXTPAD || this.type == T_TEXTPADRESET || this.type == T_CAPTIONPAD || this.type == T_CAPTIONCONFIRMPAD){
			try{ TextPad.draw(this); } catch (e){}
		}else if (this.type == T_TEXTPADNOIMG || this.type == T_TEXTPADRESETNOIMG || this.type == T_CAPTIONPADNOIMG || this.type == T_CAPTIONCONFIRMPADNOIMG){
			try{ TextPadNoImg.draw(this); } catch (e){}
		}else if (this.type == T_QUESTIONPAD){
			try{ QuestionPad.draw(this); } catch (e){}
		}else if (this.type == T_QUESTIONPADNOIMG){
			try{ QuestionPadNoImg.draw(this); } catch (e){}
		}
	};

	this.init = function f(){
		// initialize object variables from the object/array passed in
		initPad(initObj, this);
	};

	this.init();
}

function doNothing(){ return; }

function initPad (initObj, padObj){
	padObj.name=initObj.name;

	if (initObj.type != undefined){
		padObj.type = initObj.type;
	}

	if (initObj.hasCaps != undefined){
		padObj.hasCaps = ((initObj.hasCaps==true)?true:false);
	}else{
		if (padObj.type == T_KEYPAD){
			padObj.hasCaps=true;
		}
	}

	if (initObj.url != undefined){
		padObj.url = initObj.url;
		if (padObj.url != ""){
			if (padObj.url.charAt(padObj.url.length-1) != "/"){
				padObj.url += "/";
			}
			if (padObj.url.indexOf('http://') != 0 && padObj.url.indexOf('https://') != 0){
				padObj.url = 'http://' + padObj.url;
			}
		}
	}

	if (initObj.bImgLoc != undefined){
		padObj.bImgLoc = initObj.bImgLoc;
	}else{
		padObj.bImgLoc = "kbimage?pid=" + padObj.name + "&keyId=1135222081611";
	}

	if (initObj.map != undefined){
		padObj.map = initObj.map;
	}else {
		padObj.map = padObj.name + "Map";
	}

	if (initObj.keySet != undefined){
		padObj.keySet.keyCols = (initObj.keySet.keyCols!=undefined?initObj.keySet.keyCols:3);
		padObj.keySet.keyWidth = (initObj.keySet.keyWidth!=undefined?initObj.keySet.keyWidth:20);
		padObj.keySet.keyHeight = (initObj.keySet.keyHeight!=undefined?initObj.keySet.keyHeight:20);
		padObj.keySet.keyGap = (initObj.keySet.keyGap?initObj.keySet.keyGap:5);
		padObj.keySet.keys = initObj.keySet.keys;
	}

	if (initObj.captionText != undefined){
		padObj.captionText = initObj.captionText;
	}

	if (initObj.dataDivStyle != undefined){
		padObj.dataDivStyle = initObj.dataDivStyle;
	}else{
		switch(padObj.type){
			case T_KEYPAD:
				padObj.dataDivStyle = "width:110px;height:12px;left:30px;top:12px;font-size:17px;color:black;font-family:helvetica, arial;text-align:left;";
				break;
			case T_PINPAD:
				padObj.dataDivStyle = "width:70px;height:12px;left:20px;top:12px;font-size:17px;color:black;font-family:helvetica, arial;text-align:left;";
				break;
			case T_PINPADNOIMG:
				padObj.dataDivStyle = "position:absolute;left:20px;top:8px;width:62px;height:17px;padding-top:4px;padding-left:0px;border:1px black inset;font-size:17px;font-family:helvetica,arial;text-align:left;background-color:#FFFFFF;";
				break;
			case T_TEXTPAD:
			case T_TEXTPADRESET:
				padObj.dataDivStyle = "left:10px;top:55px;font-family:helvetica, arial;font-size:9px;font-weight:bold;text-align:left;";
				break;
			case T_TEXTPADNOIMG:
			case T_TEXTPADRESETNOIMG:
				padObj.dataDivStyle = "left:10px;top:55px;font-family:helvetica, arial;font-size:9px;font-weight:bold;text-align:left;";
				break;
			case T_CAPTIONPAD:
			case T_CAPTIONPADNOIMG:
				padObj.dataDivStyle = "left:10px;top:154px;font-family:helvetica, arial;font-size:9px;font-weight:bold;text-align:left;";
				break;
			case T_QUESTIONPAD:
			case T_QUESTIONPADNOIMG:
				padObj.dataDivStyle = "left:10px;top:142px;font-family:helvetica, arial;font-size:9px;font-weight:bold;text-align:left;";
				break;
		}
	}

	if (initObj.confirmDivStyle != undefined){
		padObj.confirmDivStyle = initObj.confirmDivStyle;
	} else {
		switch(padObj.type){
			case T_TEXTPADRESET:
				padObj.confirmDivStyle = "left:10px;top:105px;font-family:helvetica, arial;font-size:9px;font-weight:bold;text-align:left;";
				break;
			case T_TEXTPADRESETNOIMG:
				padObj.confirmDivStyle = "left:10px;top:105px;font-family:helvetica, arial;font-size:9px;font-weight:bold;text-align:left;";
				break;
		}
	}

	if (initObj.dataInputStyle != undefined){
		padObj.dataInputStyle = initObj.dataInputStyle;
	}else{
		switch(padObj.type){
			case T_TEXTPAD:
			case T_TEXTPADRESET:
			case T_CAPTIONPAD:
				padObj.dataInputStyle = "border:0px solid black;font-family:helvetica, arial;font-size:15px;width:125px;height:15px;";
				break;
			case T_QUESTIONPAD:
				padObj.dataInputStyle = "border:0px solid black;font-family:helvetica, arial;font-size:12px;width:128px;height:15px;";
				break;
			case T_TEXTPADNOIMG:
			case T_TEXTPADRESETNOIMG:
			case T_CAPTIONPADNOIMG:
				padObj.dataInputStyle = "border:1px solid black;font-family:helvetica, arial;font-size:15px;width:125px;height:15px;";
				break;
			case T_QUESTIONPADNOIMG:
				padObj.dataInputStyle = "border:1px solid black;font-family:helvetica, arial;font-size:12px;width:128px;height:15px;";
				break;
		}
	}

	if (initObj.dataInputType != undefined){
		padObj.dataInputType = initObj.dataInputType;
	}else{
		switch(padObj.type){
			case T_TEXTPAD:
			case T_TEXTPADRESET:
			case T_QUESTIONPAD:
				padObj.dataInputType = "password";
				break;
			case T_CAPTIONPAD:
				padObj.dataInputType = "text";
				break;
			case T_TEXTPADNOIMG:
			case T_TEXTPADRESETNOIMG:
			case T_QUESTIONPADNOIMG:
				padObj.dataInputType = "password";
				break;
			case T_CAPTIONPADNOIMG:
				padObj.dataInputType = "text";
				break;
			case T_KEYPAD:
			case T_PINPAD:
			case T_PINPADNOIMG:
			case T_CAPTIONCONFIRMPAD:
			case T_CAPTIONCONFIRMPADNOIMG:
				padObj.dataInputType = "hidden";
				break;
		}
	}
	
	if (initObj.dataLabel != undefined){
		padObj.dataLabel = initObj.dataLabel;
	} else {
		switch(padObj.type){
			case T_TEXTPAD:
			case T_TEXTPADRESET:
			case T_KEYPAD:
			case T_TEXTPADNOIMG:
			case T_TEXTPADRESETNOIMG:
				padObj.dataLabel = "Password";
				break;
			case T_CAPTIONPAD:
			case T_CAPTIONPADNOIMG:
			case T_CAPTIONCONFIRMPAD:
			case T_CAPTIONCONFIRMPADNOIMG:
				padObj.dataLabel = "Security Phrase";
				break;
			case T_PINPAD:
			case T_PINPADNOIMG:
				padObj.dataLabel = "Pin";
				break;
			case T_QUESTIONPAD:
			case T_QUESTIONPADNOIMG:
			case T_QUIZPAD:
			case T_QUIZPADNOIMG:
				padObj.dataLabel = "Answer";
				break;
		}
	}

	if (initObj.confirmInputStyle != undefined){
		padObj.confirmInputStyle = initObj.confirmInputStyle;
	}else{
		switch(padObj.type){
			case T_TEXTPADRESET:
				padObj.confirmInputStyle = "border:0px solid black;font-family:helvetica, arial;font-size:15px;width:125px;height:15px;";
				break;
			case T_TEXTPADRESETNOIMG:
				padObj.confirmInputStyle = "border:1px solid black;font-family:helvetica, arial;font-size:15px;width:125px;height:15px;";
				break;
		}
	}

	if (initObj.confirmInputType != undefined){
		padObj.confirmInputType = initObj.confirmInputType;
	}else{
		switch(padObj.type){
			case T_TEXTPAD:
			case T_TEXTPADRESET:
			case T_QUESTIONPAD:
				padObj.confirmInputType = "password";
				break;
			case T_CAPTIONPAD:
				padObj.confirmInputType = "text";
				break;
			case T_TEXTPADNOIMG:
			case T_TEXTPADRESETNOIMG:
			case T_QUESTIONPADNOIMG:
				padObj.confirmInputType = "password";
				break;
			case T_CAPTIONPADNOIMG:
				padObj.confirmInputType = "text";
				break;
			case T_KEYPAD:
			case T_PINPAD:
			case T_PINPADNOIMG:
				padObj.confirmInputType = "hidden";
				break;
		}
	}

	if (initObj.confirmLabel != undefined){
		padObj.confirmLabel = initObj.confirmLabel;
	} else {
		switch(padObj.type){
			case T_TEXTPADRESET:
			case T_TEXTPADRESETNOIMG:
				padObj.confirmLabel = "Confirm Password";
				break;
		}
	}

	if (initObj.maxLength != undefined){
		padObj.maxLength = initObj.maxLength;
	}else{
		switch(padObj.type){
			case T_KEYPAD:
				padObj.maxLength = 18;
				break;
			case T_PINPAD:
			case T_PINPADNOIMG:
				padObj.maxLength = 9;
				break;
			case T_TEXTPAD:
			case T_TEXTPADNOIMG:
			case T_TEXTPADRESET:
			case T_TEXTPADRESETNOIMG:
				padObj.maxLength = 25;
				break;
			case T_CAPTIONPAD:
			case T_CAPTIONPADNOIMG:
				padObj.maxLength = 25;
				break;
			default:
				padObj.maxLength = 20;
		}
	}

	if (initObj.userInput != undefined){
		padObj.userInput = initObj.userInput;
	}

	if (initObj.left != undefined){
		padObj.left = initObj.left + "";
	}

	if (initObj.top != undefined){
		padObj.top = initObj.top + "";
	}

	if (initObj.mapStartX != undefined){
		padObj.mapStartX = initObj.mapStartX;
	}

	if (initObj.mapStartY != undefined){
		padObj.mapStartY = initObj.mapStartY;
	}

	if (initObj.height != undefined){
		padObj.height = initObj.height + "";
	}else{ // use predefined based on pad
		padObj.height="36";

		switch(padObj.type){
			case T_KEYPAD:
				padObj.height="536";
				break;
			case T_PINPADNOIMG:
				padObj.height="223";
				break;
			case T_TEXTPADNOIMG:
			case T_TEXTPADRESETNOIMG:
			case T_CAPTIONPADNOIMG:
			case T_CAPTIONCONFIRMPADNOIMG:
			case T_QUESTIONPAD:
			case T_QUESTIONPADNOIMG:
				padObj.height="223";
				break;
		}
	}

	if (initObj.width != undefined){
		padObj.width = initObj.width + "";
	}else{ // use predefined based on pad
		switch(padObj.type){
			case T_KEYPAD:
				padObj.width="367";
				break;
			case T_PINPAD:
				padObj.width="128";
				break;
			case T_PINPADNOIMG:
				padObj.width="128";
				break;
			case T_TEXTPAD:
			case T_TEXTPADNOIMG:
			case T_TEXTPADRESET:
			case T_TEXTPADRESETNOIMG:
			case T_CAPTIONPAD:
			case T_CAPTIONPADNOIMG:
			case T_CAPTIONCONFIRMPAD:
			case T_CAPTIONCONFIRMPADNOIMG:
			case T_QUESTIONPAD:
			case T_QUESTIONPADNOIMG:
				padObj.width="148";
				break;
		}

	}


  switch(padObj.type){
  	case T_TEXTPAD:
  	case T_TEXTPADNOIMG:
	case T_TEXTPADRESET:
	case T_TEXTPADRESETNOIMG:
	case T_CAPTIONPAD:
	case T_CAPTIONPADNOIMG:
	case T_CAPTIONCONFIRMPAD:
	case T_CAPTIONCONFIRMPADNOIMG:
	case T_QUESTIONPAD:
	case T_QUESTIONPADNOIMG:
	    padObj.checkEnterKey = function f(e){
	      var characterCode;
	      if(e && e.which){
	        characterCode = e.which;
	      }else {
	        e = event;
	        characterCode = e.keyCode;
	      }
	
	      if(characterCode == 13){
	        this.keyPress('ENTERKEY');
	        return false;
	      }else {
	        return true;
	      }
	    }

  }

  if (initObj.x != undefined){
		if (!isNaN(initObj.x)){
			padObj.mouseX = initObj.x;
		}
	}
	if (initObj.y != undefined){
		if (!isNaN(initObj.y)){
			padObj.mouseY = initObj.y;
		}
	}

	padObj.adaComp = ((initObj.adaComp==true)?true:false);
	padObj.shiftDown = ((initObj.shiftDown==true)?true:false);
	padObj.capsDown = ((initObj.capsDown==true)?true:false);
	padObj.canDrag = ((initObj.canDrag==false)?false:true);
	padObj.canHide = ((initObj.canHide==false)?false:true);
	if (initObj.canHide == undefined){
		padObj.canHide = padObj.canDrag;
	}
	padObj.startVisible = ((initObj.startVisible==true)?true:false);
	if (initObj.startVisible == undefined){
		padObj.startVisible = !padObj.canHide;
	}

	padObj.submitForm = ((initObj.submitForm==false)?false:true);

	padObj.showDataDiv = ((initObj.showDataDiv==false)?false:true);
	if (padObj.type == "T_QUIZPAD" || padObj.type == "T_QUIZPADNOIMG"){
		if (initObj.showDataDiv == undefined){
			padObj.showdataDiv == false;
		}
	}
	if (!padObj.showDataDiv){
		padObj.dataDivStyle += "visibility:hidden;";
	}

	if (padObj.hasCaps){
		if (initObj.capsOnImgLoc != undefined){
			padObj.capsOnImgLoc = initObj.capsOnImgLoc;
		}

		if (initObj.capsShiftImgLoc != undefined){
			padObj.capsShiftImgLoc = initObj.capsShiftImgLoc;
		}

		if (initObj.capsOnStyle != undefined){
			padObj.capsOnStyle = initObj.capsOnStyle;
		}else{
			padObj.capsOnStyle = "left:188px;top:0px;";
		}

		if (initObj.capsShiftStyle != undefined){
			padObj.capsShiftStyle = initObj.capsShiftStyle;
		}else{
			padObj.capsShiftStyle = "left:188px;top:0px;";
		}
	}
	
	if (initObj.imgAlt != undefined){
		padObj.imgAlt = initObj.imgAlt;
	}

	padObj.draw();

	padObj.padDiv = document.getElementById(padObj.name + "PadDiv");
	padObj.dataDiv = document.getElementById(padObj.name + "DataDiv");
	if (padObj.type == T_TEXTPADRESET  || padObj.type == T_TEXTPADRESETNOIMG){
		padObj.confirmDiv = document.getElementById(padObj.name + "ConfirmDiv");
	}
	if (initObj.externalField != undefined){
		padObj.externalField = initObj.externalField;
	}

	if (padObj.type == T_PINPADNOIMG){
		padObj.keyDiv = document.getElementById(padObj.name + "KeyDiv");
		padObj.keyBGDiv = document.getElementById(padObj.name + "KeyBGDiv");
		padObj.backspaceDiv = document.getElementById(padObj.name + "BackspaceDiv");
		padObj.closeDiv = document.getElementById(padObj.name + "CloseDiv");
	}

	if (padObj.hasCaps){
		padObj.capsOnDiv = document.getElementById(padObj.name + "CapsOnDiv");
		padObj.capsShiftDiv = document.getElementById(padObj.name + "CapsShiftDiv");
	}

	if (document.getElementById(padObj.name + "DataField")){
		padObj.dataField = document.getElementById(padObj.name + "DataField");
		if (padObj.type ==T_TEXTPADRESET  || padObj.type == T_TEXTPADRESETNOIMG){
			padObj.confirmField = document.getElementById(padObj.name + "ConfirmField");
		}
		if (padObj.type != T_TEXTPAD && padObj.type != T_TEXTPADNOIMG
			&& padObj.type != T_TEXTPADRESET && padObj.type != T_TEXTPADRESETNOIMG
			&& padObj.type != T_CAPTIONPAD && padObj.type != T_CAPTIONPADNOIMG
			&& padObj.type != T_QUESTIONPAD && padObj.type != T_QUESTIONPADNOIMG ){
			padObj.dataField.value=":";
		}
	}

	if (padObj.canDrag){
		padObj.dragBar = document.getElementById(padObj.name + "DragBar");
		if (padObj.dragBar && padObj.padDiv){
			//Drag.init(padObj.dragBar, padObj.padDiv);
		}
	}

	try {
		if(padObj.type==T_PINPAD || padObj.type==T_PINPADNOIMG){
			padObj.padDiv.oncontextmenu = new Function("return false;");
		}
	} catch(exception) {}

	page_pads[padObj.name] = padObj;
  page_pads.length++;

}

var KeyPad = {
	draw : function(padObj){
		var stylePosition = ((padObj.canDrag||!padObj.startVisible)?'absolute':'relative');
		var styleVisibility = (padObj.startVisible?'visible':'hidden');
		var styleTop = padObj.top;
		var styleLeft = padObj.left;
		var styleHeight = padObj.height;
		var styleWidth = padObj.width;
		var dataStyle = padObj.dataDivStyle;
		var dataVisibility = ((padObj.startVisible && padObj.showDataDiv)?'visible':'hidden');
		var capsOnStyle = padObj.capsOnStyle;
		var capsShiftStyle = padObj.capsShiftStyle;
		var dataInputType = padObj.dataInputType;
		
		var imgAlt = padObj.imgAlt;
		
		var eventMouseTrack = ((padObj.isIE&&!padObj.isMac)?'onmousemove="' + padObj.name + '.' + 'trackMouse()"':'onclick="' + padObj.name + '.' + 'trackMouse()"');

		document.write('<div align="left" id="' + padObj.name + 'PadDiv" name="' + padObj.name + 'PadDiv" style="position: ' + stylePosition + ';width: ' + styleWidth + 'px; left: ' + styleLeft + 'px; top: ' + styleTop + 'px; visibility:' + styleVisibility + ';" ' + eventMouseTrack + '>');

		document.write('<img id="' + padObj.name + 'PadImage" src="' + padObj.url + padObj.bImgLoc + '" border="0" usemap="#' + padObj.map + '" alt="' + imgAlt + '">');
		if (padObj.hasCaps){
			document.write('<div id="' + padObj.name + 'CapsOnDiv" style="visibility:hidden;position:absolute;' + capsOnStyle + '">');
			if (padObj.capsOnImgLoc != ""){
				document.write('<a href="javascript:' + padObj.name + '.keyPress(\'CAPSLOCKKEY\',\'CAPSLOCKKEY\');" alt="CAPSLOCKKEY" title="CAPSLOCKKEY"><img border="0" src="' + padObj.url + padObj.capsOnImgLoc + '" /></a>');
			}
			document.write('</div>');

			document.write('<div id="' + padObj.name + 'CapsShiftDiv" style="visibility:hidden;position:absolute;' + capsShiftStyle + '">')
			if (padObj.capsShiftImgLoc != ""){
				document.write('<a href="javascript:' + padObj.name + '.keyPress(\'CAPSLOCKKEY\',\'CAPSLOCKKEY\');" alt="CAPSLOCKKEY" title="CAPSLOCKKEY"><img border="0" src="' + padObj.url + padObj.capsShiftImgLoc + '" /></a>');
			}
			document.write('</div>');
		}
		document.write('<div id="' + padObj.name + 'DataDiv" style="visibility:' + dataVisibility + ';position:absolute;' + dataStyle + '" align="left"></div>');

		document.write('<input type="' + dataInputType + '" name="' + padObj.name + 'DataField" id="' + padObj.name + 'DataField">');
		document.write('</div>');
	}
}

var KeyPadNoImg = {
	draw : function(padObj){ }
}

var PinPad = {
	draw : function(padObj){

		var stylePosition = ((padObj.canDrag||!padObj.startVisible)?'absolute':'relative');
		var styleVisibility = (padObj.startVisible?'visible':'hidden');
		var styleDisplay = (padObj.startVisible?'':'none');
		var styleTop = padObj.top;
		var styleLeft = padObj.left;
		var styleHeight = padObj.height;
		var styleWidth = padObj.width;
		var dataStyle = padObj.dataDivStyle;
		var dataVisibility = ((padObj.startVisible && padObj.showDataDiv)?'visible':'hidden');
		var dataInputType = padObj.dataInputType;
		var eventMouseTrack = ((padObj.isIE&&!padObj.isMac)?'onmousemove="' + padObj.name + '.' + 'trackMouse()"':'onclick="' + padObj.name + '.' + 'trackMouse()"');

		var imgAlt = padObj.imgAlt;

		document.write('<div id="' + padObj.name + 'PadDiv" name="' + padObj.name + 'PadDiv" style="display:' + styleDisplay + ';position: ' + stylePosition + ';width: ' + styleWidth + 'px; left: ' + styleLeft + 'px; top: ' + styleTop + 'px;" ' + eventMouseTrack + '>');

		document.write('<img id="' + padObj.name + 'PadImage" align="center" valign="middle" src="' + padObj.url + padObj.bImgLoc + '" border="0" usemap="#' + padObj.map + '" alt="' + imgAlt + '">');
		document.write('<div id="' + padObj.name + 'DataDiv" style="visibility:' + dataVisibility + ';position:absolute;' + dataStyle + '" align="left"></div>');

		document.write('<input type="' + dataInputType + '" name="' + padObj.name + 'DataField" id="' + padObj.name + 'DataField">');
		document.write('</div>');

	}
}

var PinPadNoImg = {
	draw : function(padObj){
		var stylePosition = ((padObj.canDrag||!padObj.startVisible)?'absolute':'relative');
		var styleVisibility = (padObj.startVisible?'visible':'hidden');
		var styleDisplay = (padObj.startVisible?'':'none');
		var styleTop = padObj.top;
		var styleLeft = padObj.left;
		var styleHeight = padObj.height;
		var styleWidth = padObj.width;
		var dataVisibility = ((padObj.startVisible && padObj.showDataDiv)?'visible':'hidden');
		var dataStyle = padObj.dataDivStyle;
		var dataInputType = padObj.dataInputType;
		var keyBGWidth = (parseInt(padObj.width,10) - (3*2) - 2);

		var keyBGHeight = 140;
		var keyCount = 0;

		var padFont = "helvetica, arial";
		var padBGColor = "#EEEFE1";
		var keyAreaBGColor = "#BEBF9D";
		var keyBGColor = "#EEEFE1";
		var controlBGColor = "#BEBF9D";

		var keyAreaPos = ({x:0, y:0});
		if (padObj.keySet != undefined){
			if (padObj.keySet.keys != undefined){
				for (var aKey in padObj.keySet.keys){
					keyCount++;
				}
			}

			keyAreaPos.x = (keyBGWidth/2) - (((padObj.keySet.keyCols * (padObj.keySet.keyWidth + padObj.keySet.keyGap)) - padObj.keySet.keyGap)/2);
			keyAreaPos.y = (keyBGHeight/2) - ((((keyCount/padObj.keySet.keyCols) * (padObj.keySet.keyHeight + padObj.keySet.keyGap)) - padObj.keySet.keyGap)/2);
		}

		document.write('<style type="text/css">');
		document.write('#' + padObj.name + 'PadDiv {display:' + styleDisplay + ';position:' + stylePosition + ';left:' + styleLeft + 'px;top:' + styleTop + 'px;height:' + styleHeight + 'px;width:' + styleWidth + 'px;background-color:' + padBGColor + ';border: 1px black solid;cursor:default;}');
		document.write('#' + padObj.name + 'DragBar {width:100%;height:40px;}');
		document.write('#' + padObj.name + 'DataDiv {visibility:' + dataVisibility + ';' + dataStyle + '}');
		document.write('#' + padObj.name + 'BackspaceDiv {visibility:' + styleVisibility + ';position:absolute;left:86px;top:8px;width:20px;height:20px;padding-top:2px;font-size:13px;font-weight:bold;font-family:' + padFont + ';text-align:center;background-color:' + controlBGColor + ';cursor:pointer;}');
		if (padObj.canHide){
			document.write('#' + padObj.name + 'CloseDiv {visibility:' + styleVisibility + ';position:absolute;left:112px;top:8px;width:10px;height:10px;padding-top:1px;font-size:7px;font-weight:bold;font-family:' + padFont + ';text-align:center;background-color:' + controlBGColor + ';cursor:pointer;}');
		}
		document.write('#' + padObj.name + 'EnterDiv {position:absolute;left:78px;top:182px;width:45px;height:18px;padding-top:2px;padding-bottom:2px;font-size:12px;font-weight:bold;font-family:' + padFont + ';text-align:center;cursor:pointer;background-color:' + controlBGColor + ';}');
		document.write('#' + padObj.name + 'CaptionDiv {position:absolute;left:12px;top:205px;text-align:left;font-size:9px;font-family:' + padFont + ';}');

		document.write('#' + padObj.name + 'KeyBGDiv {visibility:' + styleVisibility + ';position:absolute;left:3px;top:40px;width:' + keyBGWidth + 'px;height:' + keyBGHeight + 'px;text-align:left;background-color:' + keyAreaBGColor + ';}');
		document.write('#' + padObj.name + 'KeyDiv {visibility:' + styleVisibility + ';position:relative;left:' + keyAreaPos.x + 'px;top:' + keyAreaPos.y + 'px;}');
		document.write('.' + padObj.name + 'Key {position:absolute;font-size:13px;font-weight:bold;font-family:' + padFont + ';text-align:center;width:20px;cursor:pointer;background-color:' + keyBGColor + ';}');
		document.write('.' + padObj.name + 'Key a:link {text-decoration:none;color:black;}');
		document.write('.' + padObj.name + 'Key a:visited {text-decoration:none;color:black;}');
		document.write('#' + padObj.name + 'BackspaceDiv a:link {text-decoration:none;color:black;}');
		document.write('#' + padObj.name + 'BackspaceDiv a:visited {text-decoration:none;color:black;}');
		document.write('#' + padObj.name + 'EnterDiv a:link {text-decoration:none;color:black;}');
		document.write('#' + padObj.name + 'EnterDiv a:visited {text-decoration:none;color:black;}');
		document.write('#' + padObj.name + 'CloseDiv a:link {text-decoration:none;color:black;}');
		document.write('#' + padObj.name + 'CloseDiv a:visited {text-decoration:none;color:black;}');
		document.write('#' + padObj.name + 'CaptionDiv a:link {text-decoration:none;color:black;}');
		document.write('#' + padObj.name + 'CaptionDiv a:visited {text-decoration:none;color:black;}');

		document.write('</style>');
		document.write('<div id="' + padObj.name + 'PadDiv" name="' + padObj.name + 'PadDiv">');

		document.write('<div id="' + padObj.name + 'DragBar">');
		document.write('<div id="' + padObj.name + 'DataDiv"></div>');
		document.write('<div id="' + padObj.name + 'BackspaceDiv" onclick="' + padObj.name + '.keyPress(\'BSKEY\');" ondblclick="' + padObj.name + '.keyPressDbl(\'BSKEY\');"><a href="#">&lt;</a></div>');
		if(padObj.canHide){
			document.write('<div id="' + padObj.name + 'CloseDiv" onclick="' + padObj.name + '.keyPress(\'CLOSE\');"><a href="#">X</a></div>');
		}
		document.write('</div>');

		document.write('<div id="' + padObj.name + 'KeyBGDiv">');
		document.write('<div id="' + padObj.name + 'KeyDiv">');
		if (padObj.keySet.keys != undefined){
			var i=1;
			var keyTop = 0;
			var keyLeft = 0;

			for (var aKey in padObj.keySet.keys){
				document.write('<div class="' + padObj.name + 'Key" style="top:' + keyTop + 'px;left:' + keyLeft + 'px;" onclick="' + padObj.name + '.keyPress(\'' + padObj.keySet.keys[aKey].value + '\');" ondblclick="' + padObj.name + '.keyPressDbl(\'' + padObj.keySet.keys[aKey].value + '\');"><a href="#">' + (padObj.keySet.keys[aKey].key?padObj.keySet.keys[aKey].key:'&nbsp;') + '</a></div>');
				keyLeft += (padObj.keySet.keyWidth + padObj.keySet.keyGap);
				if (i%padObj.keySet.keyCols == 0){
					keyTop += (padObj.keySet.keyHeight + padObj.keySet.keyGap);
					keyLeft = 0;
				}
				i++;

			}
		}

		document.write('</div>');
		document.write('</div>');

		document.write('<div id="' + padObj.name + 'EnterDiv" onclick="' + padObj.name + '.keyPress(\'ENTERKEY\');"><a href="#">enter</a></div>');

		document.write('<div id="' + padObj.name + 'CaptionDiv"><a href="#">' + padObj.captionText + '</a></div>');
		document.write('<input type="' + dataInputType + '" name="' + padObj.name + 'DataField" id="' + padObj.name + 'DataField">');
		document.write('</div>');

	}
}

var QuizPad = {
	draw : function(padObj){
		var stylePosition = ((padObj.canDrag||!padObj.startVisible)?'absolute':'relative');
		var styleVisibility = (padObj.startVisible?'visible':'hidden');
		var styleTop = padObj.top;
		var styleLeft = padObj.left;
		var styleHeight = padObj.height;
		var styleWidth = padObj.width;

		var eventMouseTrack = ((padObj.isIE&&!padObj.isMac)?'onmousemove="' + padObj.name + '.' + 'trackMouse()"':'onclick="' + padObj.name + '.' + 'trackMouse()"');

		var imgAlt = padObj.imgAlt;

		document.write('<div id="' + padObj.name + 'PadDiv" name="' + padObj.name + 'ImgDiv" style="position: ' + stylePosition + '; width: ' + styleWidth + 'px; left: ' + styleLeft + 'px; top: ' + styleTop + 'px; visibility:' + styleVisibility + ';" ' + eventMouseTrack + '>');

		document.write('<img id="' + padObj.name + 'PadImage" align="center" valign="middle" src="' + padObj.url + padObj.bImgLoc + '" border="0" usemap="#' + padObj.map + '" alt="' + imgAlt + '">');

		document.write('<input type="hidden" name="' + padObj.name + 'DataField" id="' + padObj.name + 'DataField">');
		document.write('</div>');

	}
}

var QuizPadNoImg = {
	draw : function(padObj){

	}
}


var TextPad = {
	draw : function(padObj){

		var stylePosition = ((padObj.canDrag||!padObj.startVisible)?'absolute':'relative');
		var styleVisibility = (padObj.startVisible?'visible':'hidden');
		var styleTop = padObj.top;
		var styleLeft = padObj.left;
		var styleHeight = padObj.height;
		var styleWidth = padObj.width;

		var dataInputType = "password";
		var dataLabel = "Password";
		var dataStyleVisibility = styleVisibility;

		var imgAlt = padObj.imgAlt;

		if (padObj.type == T_CAPTIONPAD){
			dataLabel = "Caption";
		} else if (padObj.type == T_CAPTIONCONFIRMPAD){
			dataLabel = "";
			dataStyleVisibility = "hidden";
		}

		var dataStyle = padObj.dataDivStyle;
		var dataInputStyle = padObj.dataInputStyle;
		var dataInputType = padObj.dataInputType;
		

		document.write('<div id="' + padObj.name + 'PadDiv" name="' + padObj.name + 'PadDiv" style="position: ' + stylePosition + '; width: ' + styleWidth + 'px; left: ' + styleLeft + 'px; top: ' + styleTop + 'px; visibility:' + styleVisibility + ';">');
		document.write('<div id="' + padObj.name + 'DataDiv" style="visibility:' + dataStyleVisibility + ';position:absolute;' + dataStyle + '">');
		document.write('<input type="' + dataInputType + '" name="' + padObj.name + 'DataField" id="' + padObj.name + 'DataField" style="' + dataInputStyle + '" maxlength="' + padObj.maxLength + '" title="' + dataLabel + '" value="' + padObj.userInput + '" onkeypress="return ' + padObj.name + '.checkEnterKey(event);">');
		document.write('</div>');
        document.write('<img id="' + padObj.name + 'PadImage" align="center" valign="middle" src="' + padObj.url + padObj.bImgLoc + '" border="0" usemap="#' + padObj.map + '" alt="' + imgAlt + '">');

		if (padObj.type == T_TEXTPADRESET){
			var confirmStyle = padObj.confirmDivStyle;
			var confirmInputStyle = padObj.confirmInputStyle;
			var confirmInputType = padObj.confirmInputType;
			var confirmLabel = padObj.confirmLabel;
			
			document.write('<div id="' + padObj.name + 'ConfirmDiv" style="visibility:' + styleVisibility + ';position:absolute;' + confirmStyle + '">');
			document.write('<input type="' + confirmInputType + '" name="' + padObj.name + 'ConfirmField" id="' + padObj.name + 'ConfirmField" style="' + confirmInputStyle + '" maxlength="' + padObj.maxLength + '" title="' + confirmLabel + '" onkeypress="return ' + padObj.name + '.checkEnterKey(event);">');
			document.write('</div>');
		}
		document.write('</div>');
	}
}

var TextPadNoImg = {
	draw : function(padObj){
		var stylePosition = ((padObj.canDrag||!padObj.startVisible)?'absolute':'relative');
		var styleVisibility = (padObj.startVisible?'visible':'hidden');
		var styleTop = padObj.top;
		var styleLeft = padObj.left;
		var styleHeight = padObj.height;
		var styleWidth = padObj.width;

		var dataVisibility = ((padObj.startVisible && padObj.showDataDiv)?'visible':'hidden');
		var dataStyle = padObj.dataDivStyle;
		var dataInputStyle = padObj.dataInputStyle;

		var dataInputType = padObj.dataInputType;
		var dataLabel = padObj.dataLabel;
		var enterLabel = "enter";

		if (padObj.type == T_CAPTIONPAD){
			enterLabel = "enter";
		} else if (padObj.type == T_CAPTIONCONFIRMPADNOIMG){
			dataVisibility = "hidden";
			enterLabel = "enter";
		}

		var padFont = "helvetica, arial";
		var padBGColor = "#EEEFE1";
		var keyAreaBGColor = "#BEBF9D";
		var keyBGColor = "#EEEFE1";
		var controlBGColor = "#BEBF9D";

		document.write('<style type="text/css">');
		document.write('#' + padObj.name + 'PadDiv {position:' + stylePosition + ';visibility:' + styleVisibility + ';left:' + styleLeft + 'px;top:' + styleTop + 'px;height:' + styleHeight + 'px;width:' + styleWidth + 'px;background-color:' + padBGColor + ';border: 1px black solid;cursor:default;}');
		document.write('#' + padObj.name + 'DragBar {width:100%;height:100%;}');
		document.write('#' + padObj.name + 'DataDiv {visibility:' + dataVisibility + ';position:absolute;' + dataStyle + '}');
		if (padObj.type == T_TEXTPADRESETNOIMG){
			var confirmVisibility = ((padObj.startVisible && padObj.showDataDiv)?'visible':'hidden');
			var confirmStyle = padObj.confirmDivStyle;
			
			document.write('#' + padObj.name + 'ConfirmDiv {visibility:' + confirmVisibility + ';' + confirmStyle + '}');
		}
		if (padObj.canHide){
			document.write('#' + padObj.name + 'CloseDiv {visibility:' + styleVisibility + ';position:absolute;left:112px;top:8px;width:10px;height:10px;padding-top:1px;font-size:7px;font-weight:bold;font-family:' + padFont + ';text-align:center;background-color:' + controlBGColor + ';cursor:pointer;}');
		}
		document.write('#' + padObj.name + 'EnterDiv {position:absolute;left:90px;top:182px;width:45px;height:18px;padding-top:2px;padding-bottom:2px;font-size:12px;font-weight:bold;font-family:' + padFont + ';text-align:center;cursor:pointer;background-color:' + controlBGColor + ';}');
		document.write('#' + padObj.name + 'CaptionDiv {position:absolute;left:12px;top:205px;text-align:left;font-size:9px;font-family:' + padFont + ';}');

		document.write('#' + padObj.name + 'EnterDiv a:link {text-decoration:none;color:black;}');
		document.write('#' + padObj.name + 'EnterDiv a:visited {text-decoration:none;color:black;}');
		document.write('#' + padObj.name + 'CloseDiv a:link {text-decoration:none;color:black;}');
		document.write('#' + padObj.name + 'CloseDiv a:visited {text-decoration:none;color:black;}');
		document.write('#' + padObj.name + 'CaptionDiv a:link {text-decoration:none;color:black;}');
		document.write('#' + padObj.name + 'CaptionDiv a:visited {text-decoration:none;color:black;}');

		document.write('</style>');

		document.write('<div id="' + padObj.name + 'PadDiv" name="' + padObj.name + 'PadDiv">');
		document.write('<div id="' + padObj.name + 'DragBar">');
		document.write('<div id="' + padObj.name + 'DataDiv">');
		if (dataLabel != ""){
			document.write(dataLabel + ':<br>');
		}
		document.write('<input type="' + dataInputType + '" name="' + padObj.name + 'DataField" id="' + padObj.name + 'DataField" title="' + dataLabel + '" style="' + dataInputStyle + '" maxlength="' + padObj.maxLength + '" title="' + dataLabel + '" value="' + padObj.userInput + '" onkeypress="return ' + padObj.name + '.checkEnterKey(event);">');
		document.write('</div>');

		if (padObj.type == T_TEXTPADRESETNOIMG){
			var confirmInputStyle = padObj.confirmInputStyle;
			var confirmInputType = padObj.confirmInputType;
			var confirmLabel = padObj.confirmLabel;
			
			document.write('<div id="' + padObj.name + 'ConfirmDiv">');
			document.write(confirmLabel + ':<br>');
			document.write('<input type="' + confirmInputType + '" name="' + padObj.name + 'ConfirmField" id="' + padObj.name + 'ConfirmField" title="' + confirmLabel + '" style="' + confirmInputStyle + '" maxlength="' + padObj.maxLength + '" onkeypress="return ' + padObj.name + '.checkEnterKey(event);">');
			document.write('</div>');
		}

		if(padObj.canHide){
			document.write('<div id="' + padObj.name + 'CloseDiv" onclick="' + padObj.name + '.keyPress(\'CLOSE\');"><a href="#">X</a></div>');
		}
		document.write('</div>');
		document.write('<div id="' + padObj.name + 'EnterDiv" onclick="' + padObj.name + '.keyPress(\'ENTERKEY\');"><a href="#">' + enterLabel + '</a></div>');
		document.write('<div id="' + padObj.name + 'CaptionDiv"><a href="#">' + padObj.captionText + '</a></div>');
		document.write('</div>');
	}
}

var QuestionPad = {
	draw : function(padObj){

		var stylePosition = ((padObj.canDrag||!padObj.startVisible)?'absolute':'relative');
		var styleVisibility = (padObj.startVisible?'visible':'hidden');
		var styleTop = padObj.top;
		var styleLeft = padObj.left;
		var styleHeight = padObj.height;
		var styleWidth = padObj.width;

		var dataLabel = padObj.dataLabel;
		var dataStyleVisibility = styleVisibility;

		var imgAlt = padObj.imgAlt;

		var dataStyle = padObj.dataDivStyle;
		var dataInputStyle = padObj.dataInputStyle;
		var dataInputType = padObj.dataInputType;
		
		document.write('<div id="' + padObj.name + 'PadDiv" name="' + padObj.name + 'PadDiv" style="position: ' + stylePosition + '; width: ' + styleWidth + 'px; left: ' + styleLeft + 'px; top: ' + styleTop + 'px; visibility:' + styleVisibility + ';">');
		document.write('<img id="' + padObj.name + 'PadImage" align="center" valign="middle" src="' + padObj.url + padObj.bImgLoc + '" border="0" usemap="#' + padObj.map + '" alt="' + imgAlt + '">');
		document.write('<div id="' + padObj.name + 'DataDiv" style="visibility:' + dataStyleVisibility + ';position:absolute;' + dataStyle + '">');
		document.write('<input type="' + dataInputType + '" name="' + padObj.name + 'DataField" id="' + padObj.name + 'DataField" style="' + dataInputStyle + '" title="' + dataLabel  + '" onkeypress="return ' + padObj.name + '.checkEnterKey(event);" value="' + padObj.userInput + '"/>');
		document.write('</div>');

		document.write('</div>');
	}
}

var QuestionPadNoImg = {
	draw : function(padObj){
		var stylePosition = ((padObj.canDrag||!padObj.startVisible)?'absolute':'relative');
		var styleVisibility = (padObj.startVisible?'visible':'hidden');
		var styleTop = padObj.top;
		var styleLeft = padObj.left;
		var styleHeight = padObj.height;
		var styleWidth = padObj.width;

		var dataVisibility = ((padObj.startVisible && padObj.showDataDiv)?'visible':'hidden');
		var dataStyle = padObj.dataDivStyle;
		var dataInputStyle = padObj.dataInputStyle;
		var dataInputType = padObj.dataInputType
		
		var dataLabel = padObj.dataLabel;
		var enterLabel = "enter";


		var padFont = "helvetica, arial";
		var padBGColor = "#EEEFE1";
		var keyAreaBGColor = "#BEBF9D";
		var keyBGColor = "#EEEFE1";
		var controlBGColor = "#BEBF9D";

		document.write('<style type="text/css">');
		document.write('#' + padObj.name + 'PadDiv {position:' + stylePosition + ';visibility:' + styleVisibility + ';left:' + styleLeft + 'px;top:' + styleTop + 'px;height:' + styleHeight + 'px;width:' + styleWidth + 'px;background-color:' + padBGColor + ';border: 1px black solid;cursor:default;}');
		document.write('#' + padObj.name + 'DragBar {width:100%;height:100%;}');
		document.write('#' + padObj.name + 'DataDiv {visibility:' + dataVisibility + ';position:absolute;' + dataStyle + '}');
		if (padObj.canHide){
			document.write('#' + padObj.name + 'CloseDiv {visibility:' + styleVisibility + ';position:absolute;left:112px;top:8px;width:10px;height:10px;padding-top:1px;font-size:7px;font-weight:bold;font-family:' + padFont + ';text-align:center;background-color:' + controlBGColor + ';cursor:pointer;}');
		}
		document.write('#' + padObj.name + 'EnterDiv {position:absolute;left:90px;top:182px;width:45px;height:18px;padding-top:2px;padding-bottom:2px;font-size:12px;font-weight:bold;font-family:' + padFont + ';text-align:center;cursor:pointer;background-color:' + controlBGColor + ';}');
		document.write('#' + padObj.name + 'CaptionDiv {position:absolute;left:12px;top:205px;text-align:left;font-size:9px;font-family:' + padFont + ';}');

		document.write('#' + padObj.name + 'EnterDiv a:link {text-decoration:none;color:black;}');
		document.write('#' + padObj.name + 'EnterDiv a:visited {text-decoration:none;color:black;}');
		document.write('#' + padObj.name + 'CloseDiv a:link {text-decoration:none;color:black;}');
		document.write('#' + padObj.name + 'CloseDiv a:visited {text-decoration:none;color:black;}');
		document.write('#' + padObj.name + 'CaptionDiv a:link {text-decoration:none;color:black;}');
		document.write('#' + padObj.name + 'CaptionDiv a:visited {text-decoration:none;color:black;}');

		document.write('</style>');

		document.write('<div id="' + padObj.name + 'PadDiv" name="' + padObj.name + 'PadDiv">');
		document.write('<div id="' + padObj.name + 'DragBar">');
		document.write('<div id="' + padObj.name + 'DataDiv">');
		if (dataLabel != ""){
			document.write(dataLabel + ':<br>');
		}

		document.write('<input type="' + dataInputType + '" name="' + padObj.name + 'DataField" id="' + padObj.name + 'DataField" style="' + dataInputStyle + '" title="' + dataLabel  + '" onkeypress="return ' + padObj.name + '.checkEnterKey(event);" value="' + padObj.userInput + '" />');

		document.write('</div>');


		if(padObj.canHide){
			document.write('<div id="' + padObj.name + 'CloseDiv" onclick="' + padObj.name + '.keyPress(\'CLOSE\');"><a href="#">X</a></div>');
		}
		document.write('</div>');
		document.write('<div id="' + padObj.name + 'EnterDiv" onclick="' + padObj.name + '.keyPress(\'ENTERKEY\');"><a href="#">' + enterLabel + '</a></div>');
		document.write('<div id="' + padObj.name + 'CaptionDiv"><a href="#">' + padObj.captionText + '</a></div>');
		document.write('</div>');
	}
}
