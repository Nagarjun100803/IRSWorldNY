    
        /* By: Carlos H. Raymundo for PNB (C) 2009 */
    
        var jspad_canedit = true;
        var jspad_id;
        var jspad_bold = false;
        var jspad_italic = false;
        var jspad_uline = false;
        var jspad_color = "#000000";
        var jspad_foldimg = "../js/jspad/images/jspad";
        var jspad_no_colorsel = false;
        var jspad_no_toolbar = false;
        
        function jspad_get_text()
        {
            var div = document.getElementById(jspad_id + '_writepad');
            //if (div) return div.innerHTML;
            if (div)
            {
                if (div.textContent) return div.textContent;
                return div.innerText;
            }
            
            return '';
        }
        
        function jspad_set_text(msgtxt)
        {
            var div = document.getElementById(jspad_id + '_writepad');
            if (div) div.innerHTML = msgtxt;
        }
    
        function jspad_div_onclick(div)
        {
            if (jspad_canedit)
            {
                div.contentEditable = true;
                var ddl = document.getElementById(jspad_id + '_ddl_fontname');
                if (ddl)
                {
                    jspad_ddfonam_onclick(ddl);
                }
                ddl = document.getElementById(jspad_id + '_ddl_fontsize');
                if (ddl)
                {
                    jspad_ddfonsz_onclick(ddl);
                }
                if (jspad_bold)
                {
                    document.execCommand('Bold');
                    jspad_bold = false;
                }
                if (jspad_italic)
                {
                    document.execCommand('Italic');
                    jspad_italic = false;
                }
                if (jspad_uline)
                {
                    document.execCommand('Underline');
                    jspad_uline = false;
                }
                document.execCommand('ForeColor', false, jspad_color);
            }
        }
        
        function jspad_ddfonam_onclick(ddl)
        {
            var suces = document.execCommand('FontName', false, ddl.options[ddl.selectedIndex].text);
        }
        
        function jspad_ddfonsz_onclick(ddl)
        {
            var suces = document.execCommand('FontSize', false, ddl.options[ddl.selectedIndex].text);
        }
        
        function jspad_bold_click(btn)
        {
            btn.style.backgroundColor = (btn.style.backgroundColor == 'silver') ? '' : 'silver';
            jspad_bold = true;
        }
        
        function jspad_italic_click(btn)
        {
            btn.style.backgroundColor = (btn.style.backgroundColor == 'silver') ? '' : 'silver';
            jspad_italic = true;
        }
        
        function jspad_uline_click(btn)
        {
            btn.style.backgroundColor = (btn.style.backgroundColor == 'silver') ? '' : 'silver';
            jspad_uline = true;
        }
        
        function jspad_color_change(ddl)
        {
            jspad_color = ddl.value;
        }    
        
        function jspad(id, tools)
        {
            jspad_id = (id == null || id == '') ? 'divPad' : id;;
            jspad_no_toolbar = !tools;
        }
        
        jspad.prototype.fetch_fontops = function()
        {
            var htm = '';
            
            htm += '<option value=0 selected>Arial</option>\n';
            htm += '<option value=1>New Times Roman</option>\n';
            
            /*
            for (var i = 1; i < dlgHelper.fonts.count; i++)
            {
                var fon = dlgHelper.fonts(i);
                htm += '<option value="' + fon + '"';
                if (fon == 'Arial')
                {
                    htm += ' SELECTED';
                }
                htm += '>' + dlgHelper.fonts(i) + '</option>\n';
            }
            */
            
            return htm;
        }
        
        jspad.prototype.get_colorops = function()
        {
            var htm = '';
            htm += '<td style="width:18%" align=left>\n';
            htm += '<select id=' + jspad_id + '_ddl_colors  style="width: 100%" onchange="jspad_color_change(this);">\n';
            htm += '<option value="#000000" selected>Black</option>\n';
            htm += '<option value="#0000FF">Blue</option>\n';
            htm += '<option value="#FF0000">Red</option>\n';
            htm += '<option value="#008000">Green</option>\n';
            htm += '<option value="#FFFF00">Yellow</option>\n';
            htm += '<option value="#FFA500">Orange</option>\n';
            htm += '<option value="#FF00FF">Magenta</option>\n';
            htm += '</select>\n';
            htm += '</td>\n';
            
            return htm;
        }
        
        jspad.prototype.get_sizeops = function()
        {
            var htm = '';
            htm += '<td style="width:7%" align=left>\n';
            htm += '<select id=' + jspad_id + '_ddl_fontsize  style="width: 100%">\n';
            htm += '<option value=1>1</option>\n';
            htm += '<option value=2>2</option>\n';
            htm += '<option value=3 selected>3</option>\n';
            htm += '<option value=4>4</option>\n';
            htm += '<option value=5>5</option>\n';
            htm += '<option value=6>6</option>\n';
            htm += '<option value=7>7</option>\n';
            htm += '</select>\n';
            htm += '</td>\n';  
                      
            return htm;
        }
        
        jspad.prototype.get_formops = function()
        {
            var htm = '';
            htm += '<td style="width:125px; height:24px" align=left>\n';
            htm += '<img align=absmiddle src="' + jspad_foldimg + '/bold.gif" style=\"background-color:silver; border:solid 1px midnightblue; cursor:hand;" alt="Bold" onclick="jspad_bold_click(this);" />\n';
            htm += '<img align=absmiddle src="' + jspad_foldimg + '/italic.gif" style="background-color:silver; border:solid 1px midnightblue; cursor:hand;" alt="Italic" onclick="jspad_italic_click(this);" />\n';
            htm += '<img align=absmiddle src="' + jspad_foldimg + '/underline.gif" style="background-color:silver; border:solid 1px midnightblue; cursor:hand;" alt="Uline" onclick="jspad_uline_click(this);" />\n';
            htm += '</td>\n';
   
            return htm;
        }
        
        jspad.prototype.create_toolbar = function()
        {
            var htm = '';
            htm += '<tr>\n';
            htm += '<td style="width:100%; height:25px" class="navbar" align=left>';
            htm += '<table id=' + jspad_id + '_tbl_tools style="width:80%">\n';
            htm += '<tr>\n';
            htm += '<td style="width:25%" align=left>\n';
            htm += '<select id=' + jspad_id + '_ddl_fontname style="width: 100%">\n';
            htm += this.fetch_fontops();
            htm += '</select>\n';
            htm += '</td>\n';
            htm += this.get_sizeops();
            if (! jspad_no_colorsel) htm += this.get_colorops();
            htm += this.get_formops();  
            htm += '</tr>\n';
            htm += '</table>\n';
            htm += '</td>\n';            
            
            htm += '</tr>\n';

            return htm;
        }
        
        jspad.prototype.create_writepad = function()
        {
            var htm = '';
            htm += '<tr>\n';
            htm += '<td style=\"width:100%" align=center>';
            
            htm += '<div id=' + jspad_id + '_writepad style=\"width:700px; height:300px; word-wrap:break-word; overflow-y: auto; text-align:left; border: solid 1px midnightblue;\" onclick=\"jspad_div_onclick(this);\">';
            htm += '</div>';
            
            htm += '</td>';
            htm += '</tr>';
            
            return htm;
        }
        
        jspad.prototype.toString = function()
        {
            var htm = '';
            htm += "<div id=" + jspad_id + " style=\"width:710px;\">\n";
            htm += "<table id=" + jspad_id + "_tblpad width=100% class=table_body>\n";
            if (jspad_canedit && jspad_no_toolbar == false)
            {
                htm += this.create_toolbar();
            }                                                 
            htm += this.create_writepad();
            
            return htm;
        }
