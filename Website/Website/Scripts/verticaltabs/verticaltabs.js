﻿ <script type="text/javascript">

        $(function() {

            var $items = $('#vtab>ul>li');

            $items.mouseover(function() {

                $items.removeClass('selected');

                $(this).addClass('selected');

                var index = $items.index($(this));

                $('#vtab>div').hide().eq(index).show();

            }).eq(1).mouseover();

        });

    </script>
