/// <reference path="node_modules/@fortawesome/fontawesome-free/js/all.min.js" />
'use strict';

// Dependencies
var gulp = require('gulp'),
    sass = require('gulp-dart-sass'),
    minify = require('gulp-clean-css'),
    concat = require('gulp-concat'),
    browserify = require('browserify'),
    source = require('vinyl-source-stream'),
    sourcemaps = require('gulp-sourcemaps'),
    buffer = require('vinyl-buffer'),
    tsify = require('tsify'),
    terser = require('gulp-terser');

// Paths
var paths = {
    sassFiles: ['./Styles/**/*.scss', './Styles/**/*.css'],
    tsFiles: './Scripts/**/*.ts',
    css: './wwwroot/css/',
    js: './wwwroot/js/',
};


// ---------- SCSS Handling ---------- //
// SCSS to CSS Full
gulp.task('sass.compile', function () {
    return gulp.src(paths.sassFiles)
        .pipe(sass()
            .on("error", sass.logError))
        .pipe(concat('site.css'))
        .pipe(gulp.dest(paths.css));
});

// SCSS to CSS Minified
gulp.task('sass.min.compile', function () {
    return gulp.src(paths.sassFiles)
        .pipe(sass()
            .on("error", sass.logError))
        .pipe(concat('site.min.css'))
        .pipe(minify())
        .pipe(gulp.dest(paths.css));
});
// ------------------------------ //


// ---------- TS Handling ---------- //
// TS to JS - Full
gulp.task('ts.compile', function () {
    return browserify({
        basedir: '.',
        debug: true,
        entries: ['./Scripts/Site.ts'],
        cache: {},
        packageCache: {}
    })
        .plugin(tsify)
        .bundle()
        .pipe(source('site.js'))
        .pipe(buffer())
        .pipe(sourcemaps.init({ loadMaps: true }))
        .pipe(sourcemaps.write())
        .pipe(gulp.dest(paths.js));
});

// TS to JS Minified
gulp.task('ts.min.compile', function () {
    return browserify({
        basedir: '.',
        debug: true,
        entries: ['./Scripts/Site.ts'],
        cache: {},
        packageCache: {}
    })
        .plugin(tsify)
        .bundle()
        .pipe(source('site.min.js'))
        .pipe(buffer())
        .pipe(terser())
        .pipe(gulp.dest(paths.js));
});
// ------------------------------ //


// Bootstrap
gulp.task('import.bootstrap', function () {
    return gulp.src('./node_modules/bootstrap/dist/**/*')
        .pipe(gulp.dest('./wwwroot/lib/bootstrap/dist/'))
});


// Font Awesome
gulp.task('import.fontawesome', function () {
    return gulp.src('./node_modules/@fortawesome/fontawesome-free/js/all.min.js')
        .pipe(gulp.dest('./wwwroot/lib/fontawesome-free/dist/'))
});


// JQuery
gulp.task('import.jquery', function () {
    return gulp.src('./node_modules/jquery/dist/**/*')
        .pipe(gulp.dest('./wwwroot/lib/jquery/dist/'));
});


// JQuery Validation
gulp.task('import.jquery-validation', function () {
    return gulp.src('./node_modules/jquery-validation/dist/**/*')
        .pipe(gulp.dest('./wwwroot/lib/jquery-validation/dist/'));
});


// JQuery Validation Unobtrusive
gulp.task('import.jquery-validation-unobtrusive', function () {
    return gulp.src('./node_modules/jquery-validation-unobtrusive/dist/**/*')
        .pipe(gulp.dest('./wwwroot/lib/jquery-validation-unobtrusive/dist/'));
});


// Slick Carousel
gulp.task('import.slick-carousel', function (done) {
    gulp.src('./node_modules/slick-carousel/slick/slick.css')
        .pipe(gulp.dest('./wwwroot/lib/slick-carousel/dist/css/'));

    gulp.src('./node_modules/slick-carousel/slick/slick-theme.css')
        .pipe(gulp.dest('./wwwroot/lib/slick-carousel/dist/css/'));

    gulp.src('./node_modules/slick-carousel/slick/fonts/**/*')
        .pipe(gulp.dest('./wwwroot/lib/slick-carousel/dist/css/fonts'));

    gulp.src('./node_modules/slick-carousel/slick/slick.min.js')
        .pipe(gulp.dest('./wwwroot/lib/slick-carousel/dist/js/'));

    done();
});


// Import All
gulp.task('import.all',
    gulp.series('import.bootstrap', 'import.fontawesome', 'import.jquery', 'import.jquery-validation', 'import.jquery-validation-unobtrusive', 'import.slick-carousel'),
    function () {}
);


// Autorun
gulp.task('watch.sass.compile', function () {
    gulp.watch(paths.sassFiles, gulp.series('sass.compile'));
    gulp.watch(paths.sassFiles, gulp.series('sass.min.compile'));
});

gulp.task('watch.ts.compile', function () {
    gulp.watch(paths.tsFiles, gulp.series('ts.compile'));
    gulp.watch(paths.tsFiles, gulp.series('ts.min.compile'));
});


//Initialise
gulp.task('initialise',
    gulp.series('import.all', 'sass.compile', 'sass.min.compile', 'ts.compile', 'ts.min.compile'),
    function () {}
);