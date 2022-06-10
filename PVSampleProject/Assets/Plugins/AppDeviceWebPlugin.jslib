mergeInto(LibraryManager.library, 
{
  DeviceInformation: function () 
  {
    // OSの判定.
    var os = "";

    // iPhone,iPod.
    if( navigator.userAgent.indexOf('iPhone') > 0 || navigator.userAgent.indexOf('iPod') > 0 )
    {
      // iosスマホ用の処理
      // SendMessage('AppGameManager', 'SetDevice', "iPhone" );
      SendMessage('HtmlReceiver', 'OnHTML_SetDevice', "iPhone" );
      os = "iOS";
    }
    // Android&Mobile.
    else if ( navigator.userAgent.indexOf('Android') > 0 && navigator.userAgent.indexOf('Mobile') > 0 )
    {
      // Androidスマホ用の処理
      // SendMessage('AppGameManager', 'SetDevice', "android" );
      SendMessage('HtmlReceiver', 'OnHTML_SetDevice', "android" );
      os = "android";
    }
    // iPad.
    else if(navigator.userAgent.indexOf('iPad') > 0 )
    {
      //タブレット用の処理
      // SendMessage('AppGameManager', 'SetDevice', "iPad" );
      SendMessage('HtmlReceiver', 'OnHTML_SetDevice', "iPad" );
      os = "iOS";
    }
    // Android(Not Mobile).
    else if( navigator.userAgent.indexOf('Android') > 0 ) 
    {
      //SendMessage('AppGameManager', 'SetDevice', "androidTab" );
      SendMessage('HtmlReceiver', 'OnHTML_SetDevice', "androidTab" );
      os = "android";
    } 
    // Throw iPad Safari, Not Chrome.
    else if (navigator.userAgent.indexOf('Safari') > 0 && navigator.userAgent.indexOf('Chrome') == -1 && typeof document.ontouchstart !== 'undefined')
    {
      // iOS13以降のiPad用の処理
      // SendMessage('AppGameManager', 'SetDevice', "iOSOther" );
      SendMessage('HtmlReceiver', 'OnHTML_SetDevice', "iOSOther" );
      os = "iOS";
    }
    else
    {
      //SendMessage('AppGameManager', 'SetDevice', navigator.userAgent );
      SendMessage('HtmlReceiver', 'OnHTML_SetDevice', navigator.userAgent );
    }
  },


  OSInformation: function()
  {
    var ua = window.navigator.userAgent.toLowerCase();

    if(ua.indexOf("windows nt") !== -1) 
    {
      console.log("<<Play on Windows>>");
      // SendMessage('AppGameManager', 'SetOS', "Windows" );
      SendMessage('HtmlReceiver', 'OnHTML_SetOS', "Windows" );
    } 
    else if(ua.indexOf("android") !== -1) 
    {
      console.log("<<Play on android>>");
      // SendMessage('AppGameManager', 'SetOS', "android" );
      SendMessage('HtmlReceiver', 'OnHTML_SetOS', "android" );
    } 
    else if(ua.indexOf("iphone") !== -1 || ua.indexOf("ipad") !== -1) 
    {
      console.log("<<Play on iOS>>");
      // SendMessage('AppGameManager', 'SetOS', "iOS" );
      SendMessage('HtmlReceiver', 'OnHTML_SetOS', "iOS" );
    } 
    else if(ua.indexOf("mac os x") !== -1) 
    {
      console.log("<<Play on OSX>>");
      // SendMessage('AppGameManager', 'SetOS', "OSX" );
      SendMessage('HtmlReceiver', 'OnHTML_SetOS', "OSX" );
    } 
    else 
    {
      console.log("<<Play on UnknownOS>>");
      // SendMessage('AppGameManager', 'SetOS', "Unknown" );
      SendMessage('HtmlReceiver', 'OnHTML_SetOS', "Unknown" );
    }
  },


  
  OpenUrl: function( url )
  {
    var log = "ブースを離れページを移動します。";
    var caution = log + "(" + Pointer_stringify( url ) + ")";
    window.alert( caution );
    window.location.href = Pointer_stringify( url ); 
  },

  OpenUrlNewWindow: function( url )
  {
    var log = "新しいタブを開きます。";
    var caution = log + "(" + Pointer_stringify( url ) + ")";
    window.alert( caution );
    window.open( Pointer_stringify( url ) );
  },

  OpenHeader: function()
  {
    //window.alert( "Open" );
    var header = document.querySelector("#header");
    header.style.display = "flex";
  },

  CloseHeader: function()
  {
    //window.alert( "Close" );
    var header = document.querySelector("#header");
    header.style.display = "none";
  },

 

  SetMute: function( isMute )
  {
    var muteImg = document.getElementById("buttonImageMute");

    if( isMute == true )
    {
      // Mute中.
      muteImg.src = "TemplateData/muteButton.png";
      // SendMessage('HtmlReceiver', 'OnHTML_SetMute', true );
    }
    else
    {
      // Muteじゃない.
      muteImg.src = "TemplateData/muteOff.png";
      // SendMessage('HtmlReceiver', 'OnHTML_SetMute', false );
    }
  },

  SetMenuIconDisplay: function( isDisplay )
  {
    var menuButton = document.querySelector("#menuButton");
    if( isDisplay == true ) 
    {
      menuButton.style.display = "block"
    }
    else
    {
      menuButton.style.display = "none"
    }
  },

  SetSoundIconDisplay: function( isDisplay )
  {
    var soundButton = document.querySelector("#soundButton");
    if( isDisplay == true ) 
    {
      soundButton.style.display = "block"
    }
    else
    {
      soundButton.style.display = "none"
    }
  },

  SetMuteIconDisplay: function( isDisplay )
  {
    var muteButton = document.querySelector("#muteButton");
    if( isDisplay == true ) 
    {
      muteButton.style.display = "block"
    }
    else
    {
      muteButton.style.display = "none"
    }
  }





/* ----------------------------------------------------------------------------------- 参考..

  WatchDeviceorientation: function () 
  {
    // OSの判定.
    var os = "";

    // iPhone,iPod.
    if( navigator.userAgent.indexOf('iPhone') > 0 || navigator.userAgent.indexOf('iPod') > 0 )
    {
      // iosスマホ用の処理
      SendMessage('LocationManager', 'SetDevice', "iPhone" );
      os = "iOS";
    }
    // Android&Mobile.
    else if ( navigator.userAgent.indexOf('Android') > 0 && navigator.userAgent.indexOf('Mobile') > 0 )
    {
      // Androidスマホ用の処理
      SendMessage('LocationManager', 'SetDevice', "android" );
      os = "android";
    }
    // iPad.
    else if(navigator.userAgent.indexOf('iPad') > 0 )
    {
      //タブレット用の処理
      SendMessage('LocationManager', 'SetDevice', "iPad" );
      os = "iOS";
    }
    // Android(Not Mobile).
    else if( navigator.userAgent.indexOf('Android') > 0 ) 
    {
      SendMessage('LocationManager', 'SetDevice', "androidTab" );
      os = "android";
    } 
    // Throw iPad Safari, Not Chrome.
    else if (navigator.userAgent.indexOf('Safari') > 0 && navigator.userAgent.indexOf('Chrome') == -1 && typeof document.ontouchstart !== 'undefined')
    {
      // iOS13以降のiPad用の処理
      SendMessage('LocationManager', 'SetDevice', "iOSOther" );
      os = "iOS";
    }


    // ジャイロセンサーが使えるかどうか
    if (window.DeviceOrientationEvent) 
    {
      //window.alert( permissionState );

      // IOS.
      if( os == "iOS" )
      {
        DeviceOrientationEvent.requestPermission()  
        
        // ジャイロのを測定.
        window.addEventListener
        (
          "deviceorientation", 
          function(event) 
          {
            if( event != null )
            {
              var a = event.alpha;
              var b = event.beta;
              var g = event.gamma;

              // ジャイロ値を渡す.
              if( a != null ) SendMessage('LocationManager', 'ShowRotationAlpha', a );
              if( b != null ) SendMessage('LocationManager', 'ShowRotationBeta', b );
              if( g != null ) SendMessage('LocationManager', 'ShowRotationGamma', g );

              //var compass = event.webkitCompassHeading;
              // コンパス値を渡す.
              //SendMessage('LocationManager', 'ShowWebkitCompassHeading_iOS', compass );
            }
          }
        );  
      }
      // Android.
      else if( os == "android" )
      {   
        // ジャイロのを測定.
        window.addEventListener
        (
          "deviceorientationabsolute", 
          function(event) 
          {
            
            if( event != null )
            {
              var a = event.alpha;
              var b = event.beta;
              var g = event.gamma;

              // ジャイロ値を渡す.
              if( a != null ) SendMessage('LocationManager', 'ShowRotationAlpha', a );
              if( b != null ) SendMessage('LocationManager', 'ShowRotationBeta', b );
              if( g != null ) SendMessage('LocationManager', 'ShowRotationGamma', g );
            }

          }
        );      
      }
      else
      {
        // window.alert( "iOS,Android以外では計測できません" );
        window.alert( "OS情報が取得できないため計測ができません" );
      }
    }
    else
    {
      SendMessage('LocationManager', 'SetError', "傾き取得ができません");
    }   
  },



  Alert: function () {
    window.alert(" テスト表示 ");
  }

------------------------------------------------------------------------------------------------------ */

  
});