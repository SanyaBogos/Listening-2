﻿/// <binding AfterBuild='angularCompile' />
"use strict";

var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    rename = require("gulp-rename"),
    htmlmin = require("gulp-htmlmin"),
    ngTemplates = require("gulp-ng-templates"),
    uglify = require("gulp-uglify");

var templateCache = require('gulp-angular-templatecache');
var ngAnnotate = require('gulp-ng-annotate');
var ngmin = require('gulp-ngmin');
var watch = require('gulp-watch');
var brouserify = require('gulp-browserify');
var less = require('gulp-less');
var minifyCSS = require('gulp-minify-css');

var paths = {
    webroot: "./wwwroot/",
    style: './Styles/'
};

paths.js = paths.webroot + "js/**/*.js";
paths.minJs = paths.webroot + "js/**/*.min.js";
paths.css = paths.style + "css/**/*.css";
paths.minCss = paths.webroot + "css/**/*.min.css";
paths.concatJsDest = paths.webroot + "js/site.min.js";
paths.concatCssDest = paths.webroot + "css/site.min.css";

paths.angularJsSrc = 'Scripts/app/**/*.js';
paths.angularHtmlSrc = 'Scripts/**/*.html';
paths.angularDest = paths.webroot + 'js/angular/';
paths.lessSrc = 'Styles/**/*.less';
paths.lessDst = paths.webroot + 'css/';


gulp.task("clean:js", function (cb) {
    rimraf(paths.concatJsDest, cb);
});

gulp.task("clean:css", function (cb) {
    rimraf(paths.concatCssDest, cb);
});

gulp.task("clean:angular", function (cb) {
    rimraf(paths.angularDest, cb);
});

gulp.task("clean", ["clean:js", "clean:css", "clean:angular"]);

gulp.task("min:js", function () {
    return gulp.src([paths.js, "!" + paths.minJs], { base: "." })
        .pipe(concat(paths.concatJsDest))
        .pipe(uglify())
        .pipe(gulp.dest("."));
});

gulp.task("min:css", function () {
    return gulp.src([paths.css, "!" + paths.minCss])
        .pipe(concat(paths.concatCssDest))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});

gulp.task("min", ["min:js", "min:css"]);

gulp.task("copy:angularJs", function () {
    return gulp.src([paths.angularJsSrc])
        .pipe(ngAnnotate())
        .pipe(gulp.dest(paths.angularDest));
});

gulp.task('templates', ['clean'], function () {
    return gulp.src('Scripts/app/**/*.html')
		.pipe(htmlmin({
		    collapseBooleanAttributes: true,
		    collapseWhitespace: true,
		    removeAttributeQuotes: true,
		    removeComments: true,
		    removeEmptyAttributes: true,
		    removeRedundantAttributes: true,
		    removeScriptTypeAttributes: true,
		    removeStyleLinkTypeAttributes: true
		}))
		.pipe(ngTemplates({
		    filename: 'templates.js',
		    module: 'templates',
		    path: function (path, base) {
		        return path.replace(base, '')
		            .replace('templates', '');
		    }
		}))
		.pipe(gulp.dest(paths.angularDest));
});


gulp.task("angularConcat", ['templates'], function () {
    return gulp.src([paths.angularJsSrc, paths.angularDest])
        .pipe(ngAnnotate())
        .pipe(concat('app.js'))
        .pipe(gulp.dest(paths.angularDest));
});

gulp.task('min:angular', ['angularConcat'], function () {
    return gulp.src(paths.angularDest + 'app.js')
		.pipe(ngmin())
        .pipe(uglify({ mangle: false }))
        .pipe(rename({ suffix: '.min' }))
		.pipe(gulp.dest(paths.angularDest));
});

gulp.task("less", function () {
    return gulp.src(paths.lessSrc)
        .pipe(less())
        .pipe(minifyCSS())
        .pipe(rename({ suffix: '.min' }))
        .pipe(gulp.dest(paths.lessDst));
});

gulp.task("angularCompile", ["min:angular", "less", "min:css"]);

gulp.task("watcher", function () {
    //gulp.watch(paths.angularJsSrc, ["angularCopy"]);
    gulp.watch([paths.angularJsSrc, paths.angularHtmlSrc, paths.lessSrc], ["angularCompile"]);
});
