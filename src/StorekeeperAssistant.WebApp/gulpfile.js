/// <binding />
"use strict";

var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    sass = require("gulp-sass"),
    uglify = require("gulp-uglify-es").default;

var paths = {
    webroot: "./wwwroot/",
    noderoot: "./node_modules/"
};

// plugins js
paths.concatPluginJsDest = paths.webroot + "js/plugin.min.js";

paths.jqueryJs = paths.noderoot + "jquery/dist/jquery.min.js";
paths.bootstrapJs = paths.noderoot + "bootstrap/dist/js/bootstrap.bundle.min.js";
paths.axiosJs = paths.noderoot + "axios/dist/axios.min.js";
paths.momentJs = paths.noderoot + "moment/min/moment-with-locales.min.js";
paths.vueJs = paths.noderoot + "vue/dist/vue.min.js";
paths.vuejsPaginateJs = paths.noderoot + "vuejs-paginate/dist/index.js";
paths.vueSelectJs = paths.noderoot + "vue-select/dist/vue-select.js";
paths.fontAwesomeJs = paths.noderoot + "@fortawesome/fontawesome-free/js/all.js";
paths.lodashJs = paths.noderoot + "lodash/lodash.min.js";
paths.vueCtkDateTimePickerJs = paths.noderoot + "vue-ctk-date-time-picker/dist/vue-ctk-date-time-picker.umd.min.js";

gulp.task("plugin.min:js", function () {
    return gulp.src([
        paths.jqueryJs,
        paths.bootstrapJs,
        paths.vueJs,
        paths.axiosJs,
        paths.momentJs,
        paths.vuejsPaginateJs,
        paths.vueCtkDateTimePickerJs,
        paths.vueSelectJs,
        paths.fontAwesomeJs,
        paths.lodashJs
    ], { base: "." })
        .pipe(concat(paths.concatPluginJsDest))
        .pipe(uglify())
        .pipe(gulp.dest("."));
});


// plugins css
paths.concatPluginCssDest = paths.webroot + "css/plugin.min.css";

paths.bootstrapCss = paths.noderoot + "bootstrap/dist/css/bootstrap.min.css";
paths.fontAwesomeFonts = paths.noderoot + '@fortawesome/fontawesome-free/webfonts/*';
paths.vueSelectCss = paths.noderoot + "vue-select/dist/vue-select.css";
paths.vueCtkDateTimePickerCss = paths.noderoot + "vue-ctk-date-time-picker/dist/vue-ctk-date-time-picker.css";

gulp.task("plugin.min:css", function () {
    return gulp.src([
        paths.bootstrapCss,
        paths.fontAwesomeFonts,
        paths.vueSelectCss,
        paths.vueCtkDateTimePickerCss
    ])
        .pipe(concat(paths.concatPluginCssDest))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});

// fonts 
paths.fonts = paths.webroot + "fonts";

paths.fontAwesomeFonts = paths.noderoot + 'font-awesome/fonts/*';

gulp.task('fonts', function () {
    return gulp.src([
        paths.fontAwesomeFonts
    ])
        .pipe(gulp.dest(paths.fonts));
});