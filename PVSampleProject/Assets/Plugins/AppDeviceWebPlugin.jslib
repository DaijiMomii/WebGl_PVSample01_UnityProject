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
      SendMessage('AppGameManager', 'SetPlatform', "iPhone" );
      os = "iOS";
    }
    // Android&Mobile.
    else if ( navigator.userAgent.indexOf('Android') > 0 && navigator.userAgent.indexOf('Mobile') > 0 )
    {
      // Androidスマホ用の処理
      SendMessage('AppGameManager', 'SetPlatform', "android" );
      os = "android";
    }
    // iPad.
    else if(navigator.userAgent.indexOf('iPad') > 0 )
    {
      //タブレット用の処理
      SendMessage('AppGameManager', 'SetPlatform', "iPad" );
      os = "iOS";
    }
    // Android(Not Mobile).
    else if( navigator.userAgent.indexOf('Android') > 0 ) 
    {
      SendMessage('AppGameManager', 'SetPlatform', "androidTab" );
      os = "android";
    } 
    // Throw iPad Safari, Not Chrome.
    else if (navigator.userAgent.indexOf('Safari') > 0 && navigator.userAgent.indexOf('Chrome') == -1 && typeof document.ontouchstart !== 'undefined')
    {
      // iOS13以降のiPad用の処理
      SendMessage('AppGameManager', 'SetPlatform', "iOSOther" );
      os = "iOS";
    }
    else
    {
      SendMessage('AppGameManager', 'SetPlatform', navigator.userAgent );
    }
  },



  
  OpenUrl: function( url )
  {
    window.alert( "アクセス" );
    window.alert( Pointer_stringify( url ) );
    window.location.href = Pointer_stringify( url ); 
  },





  WatchDeviceorientation: function () 
  {
    // OSの判定.
    var os = "";

    // iPhone,iPod.
    if( navigator.userAgent.indexOf('iPhone') > 0 || navigator.userAgent.indexOf('iPod') > 0 )
    {
      // iosスマホ用の処理
      SendMessage('LocationManager', 'SetPlatform', "iPhone" );
      os = "iOS";
    }
    // Android&Mobile.
    else if ( navigator.userAgent.indexOf('Android') > 0 && navigator.userAgent.indexOf('Mobile') > 0 )
    {
      // Androidスマホ用の処理
      SendMessage('LocationManager', 'SetPlatform', "android" );
      os = "android";
    }
    // iPad.
    else if(navigator.userAgent.indexOf('iPad') > 0 )
    {
      //タブレット用の処理
      SendMessage('LocationManager', 'SetPlatform', "iPad" );
      os = "iOS";
    }
    // Android(Not Mobile).
    else if( navigator.userAgent.indexOf('Android') > 0 ) 
    {
      SendMessage('LocationManager', 'SetPlatform', "androidTab" );
      os = "android";
    } 
    // Throw iPad Safari, Not Chrome.
    else if (navigator.userAgent.indexOf('Safari') > 0 && navigator.userAgent.indexOf('Chrome') == -1 && typeof document.ontouchstart !== 'undefined')
    {
      // iOS13以降のiPad用の処理
      SendMessage('LocationManager', 'SetPlatform', "iOSOther" );
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