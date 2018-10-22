var gulp = require('gulp'),
    fs = require('fs'),
    sass = require('gulp-sass'),
    watch = require('gulp-watch');

gulp.task("sass", function () {
    return gulp.src('./Styles/style.scss')
        .pipe(sass()).on('error', sass.logError)
        .pipe(gulp.dest('./wwwroot/css'));
});

gulp.task("watch", function () {
    watch('./Styles/*.scss', function () {
        gulp.start('sass');
    });
});