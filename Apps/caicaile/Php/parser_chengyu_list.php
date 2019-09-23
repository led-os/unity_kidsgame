<?php
header("Content-type: text/html; charset=utf-8");

include('./parser_chengyu_item.php');


function PaserList()
{
    $json_string = file_get_contents('chengyu_list.json');
    $data = json_decode($json_string, true);
    $array = $data['items'];
    foreach ($array as $item) {
        $url = $item['url'];
        if (!BlankString($url)) {
            ParserChengyuItem($url, "chengyu/" . $item['id'] . ".json");
        }
    }
}

PaserList();
echo 'done<br>';
