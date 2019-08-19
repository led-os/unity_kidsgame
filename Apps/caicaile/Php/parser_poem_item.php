<?php
header("Content-type: text/html; charset=utf-8");

include('./simple_html_dom.php');

$is_find_poem = false;
$listPoem = array();
/*
    百度百科 唐诗三百首
    https://baike.baidu.com/item/%E5%94%90%E8%AF%97%E4%B8%89%E7%99%BE%E9%A6%96/18677

    item:
    https://baike.baidu.com/item/%E8%B4%BC%E9%80%80%E7%A4%BA%E5%AE%98%E5%90%8F/6960505?fromtitle=%E8%B4%BC%E9%80%80%E7%A4%BA%E5%AE%98%E5%90%8F%C2%B7%E5%B9%B6%E5%BA%8F&fromid=9424895
    
    */

function get_html($url)
{
    $html = new simple_html_dom();

    // // 从url中加载  
    // $html->load_file('http://www.jb51.net');  

    // // 从字符串中加载  
    // $html->load('<html><body>从字符串中加载html文档演示</body></html>');  

    //从文件中加载  
    $html->load_file($url);

    return $html;
}

/*
删除HTML标签和标签的内容。

使用方法：strip_html_tags($tags,$str)

$tags：需要删除的标签（数组格式）
$str：需要处理的字符串；
*/
function strip_html_tags($tags, $str)
{
    $html = array();
    foreach ($tags as $tag) {
        $html[] = '/<' . $tag . '.*?>[\s|\S]*?<\/' . $tag . '>/';
        $html[] = '/<' . $tag . '.*?>/';
    }
    $data = preg_replace($html, '', $str);
    return $data;
}

function GetPoemItem($div)
{
    global $is_find_poem;
    global $listPoem;
    $a = $div->find('a[target=_blank]', 0);
    if (!$a) {
        //  echo "find array_a fail \n";
        return;
    }
    //<b>五言古诗</b>
    // foreach ($array_a as $a) 

    {
        $b = $a->find('b', 0);
        if ($b) {
            $is_find_poem = true;
            // echo $a->innertext;
            echo $b->innertext;
            echo "\n";
        } else {
            if ($is_find_poem) {
                echo $a->innertext;
                echo "\n";
                echo $a->href;
                echo "\n";


                $element = array(
                    'id' => urlencode($a->innertext),
                    'url' => urlencode($a->href),
                );
                array_push($listPoem, $element);

                if ($a->innertext == "同题仙游观") {
                    //end
                    $is_find_poem = false;
                }
            }
        }
    }
}
function parserHtml($url, $save_file)
{
    // get DOM from URL or file
    $html = get_html($url);
    if (!$html) {
        echo "open html fail";
        return;
    }
    //<table class="table media-table js-media-details">
    $div_main = $html->find('div[class=main-content]', 0);
    if (!$div_main) {
        echo "find div_main fail";
        return;
    }
    echo "find div_main\n\n";


    //原文
    //start <div class="anchor-list">
    //end  <div class="anchor-list">
    $class_start = "anchor-list";
    $isFindItem = false;
    $indexClass = 0;

    $array_div = $div_main->find('div');
    if (!$array_div) {
        echo "find array_div fail";
        return;
    }
    foreach ($array_div as $div) {
        if ($div->class ==  $class_start) {
            if ($indexClass == 0) {
                $isFindItem = true;
            } else {
                $isFindItem = false;
            }

            $indexClass++;
        }
        /*
tag – 返回html标签名

innertext – 返回innerHTML

outertext – 返回outerHTML

plaintext – 返回html标签中的文本
*/

        if ($isFindItem) {
            if ($div->class ==  "para") {
                // echo $div->plaintext;
                $str = $div->innertext;

                //删除相关标签和内容
                $str = strip_html_tags(array('sup', 'a', 'b'), $str);

                //删除标签:i
                $str = strip_tags($str);
                
                echo $str;
                echo "\n";
            }
        }
    }

    global $listPoem;

    $arr = array('Item' => $listPoem);
    $jsn = urldecode(json_encode($arr));

    $fp = fopen($save_file, "w");
    if (!$fp) {
        echo "打开文件失败<br>";
        return;
    }
    $flag = fwrite($fp, $jsn);
    if (!$flag) {
        echo "写入文件失败<br>";
    }
    fclose($fp);
}



parserHtml('https://baike.baidu.com/item/%E8%B4%BC%E9%80%80%E7%A4%BA%E5%AE%98%E5%90%8F%C2%B7%E5%B9%B6%E5%BA%8F', "poem_item.json");
//parserAd('../gdt_hd.html',"../gdt_hd.json");
echo 'done<br>';
