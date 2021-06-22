/**
 * @license Copyright (c) 2003-2013, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.html or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
         config.language = 'fa';
    // config.uiColor = '#AADC6E';
        
    config.contentsCss = 'fonts/fonts.css';
    config.font_names = 'B Titr/B Titr;' + config.font_names;
    

    config.toolbar = [
 
    { items: ['Templates', 'clipboard', 'Cut', 'Paste', 'Redo', 'Undo', 'Find', '-', 'basicstyles', 'cleanup', 'Link', 'Unlink', 'Image', 'Table', 'SpecialChar', 'Syntaxhighlight', 'HorizontalRule', 'PageBreak', 'ShowBlocks', '-', 'Maximize', 'Preview'] },
    { items: ['Format', 'Font', 'FontSize', '-', 'Bold', 'Italic', 'Underline', 'Strike', '-', 'Subscript', 'Superscript', '-', 'NumberedList', 'BulletedList', 'Indent', 'Outdent', '-', 'JustifyBlock', 'JustifyRight', 'JustifyCenter', 'JustifyLeft', 'BidiRtl', 'BidiLtr', 'TextColor', 'BGColor', 'Source'] }
    ];
};
