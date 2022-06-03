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
      SendMessage('AppGameManager', 'SetDevice', "iPhone" );
      os = "iOS";
    }
    // Android&Mobile.
    else if ( navigator.userAgent.indexOf('Android') > 0 && navigator.userAgent.indexOf('Mobile') > 0 )
    {
      // Androidスマホ用の処理
      SendMessage('AppGameManager', 'SetDevice', "android" );
      os = "android";
    }
    // iPad.
    else if(navigator.userAgent.indexOf('iPad') > 0 )
    {
      //タブレット用の処理
      SendMessage('AppGameManager', 'SetDevice', "iPad" );
      os = "iOS";
    }
    // Android(Not Mobile).
    else if( navigator.userAgent.indexOf('Android') > 0 ) 
    {
      SendMessage('AppGameManager', 'SetDevice', "androidTab" );
      os = "android";
    } 
    // Throw iPad Safari, Not Chrome.
    else if (navigator.userAgent.indexOf('Safari') > 0 && navigator.userAgent.indexOf('Chrome') == -1 && typeof document.ontouchstart !== 'undefined')
    {
      // iOS13以降のiPad用の処理
      SendMessage('AppGameManager', 'SetDevice', "iOSOther" );
      os = "iOS";
    }
    else
    {
      SendMessage('AppGameManager', 'SetDevice', navigator.userAgent );
    }
  },


  OSInformation: function()
  {
    var ua = window.navigator.userAgent.toLowerCase();

    if(ua.indexOf("windows nt") !== -1) 
    {
      console.log("<<Play on Windows>>");
      SendMessage('AppGameManager', 'SetOS', "Windows" );
    } 
    else if(ua.indexOf("android") !== -1) 
    {
      console.log("<<Play on android>>");
      SendMessage('AppGameManager', 'SetOS', "android" );
    } 
    else if(ua.indexOf("iphone") !== -1 || ua.indexOf("ipad") !== -1) 
    {
      console.log("<<Play on iOS>>");
      SendMessage('AppGameManager', 'SetOS', "iOS" );
    } 
    else if(ua.indexOf("mac os x") !== -1) 
    {
      console.log("<<Play on OSX>>");
      SendMessage('AppGameManager', 'SetOS', "OSX" );
    } 
    else 
    {
      console.log("<<Play on UnknownOS>>");
      SendMessage('AppGameManager', 'SetOS', "Unknown" );
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

  OpenMenu: function()
  {
    window.alert( "Open" );
    var menu = document.querySelector("#menu");
    menu.style.display = "flex";
  },

  CloseMenu: function()
  {
    window.alert( "Close" );
    var menu = document.querySelector("#menu");
    menu.style.display = "none";
  },

  SetMenuState: function( state )
  {
    var soundButton = document.querySelector("#soundButton");
    var muteImg = document.getElementById("buttonImageMute");
    var _state = Pointer_stringify( state )

    if( _state == "Mute" || _state == "mute" )
    {
      // Mute中（MenuボタンとMute解除ボタン）.
      muteImg.src = "TemplateData/muteButton.png";
      soundButton.style.display = "none";
    }
    else
    {
      // Muteじゃない（Menuボタン、音量ボタン、Muteボタン）.
      muteImg.src = "TemplateData/muteOff.png";
      soundButton.style.display = "block";
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



  
});