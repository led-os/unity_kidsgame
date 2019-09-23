<?php
header("Content-type: text/html; charset=utf-8");

function FileIsExist($filepath)
{
    $ret =file_exists($filepath);
    return $ret;
}
 
 
 
