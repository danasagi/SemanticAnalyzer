// Copyright (c) 2014 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

/**
 * Get the current URL.
 *
 * @param {function(string)} callback - called when the URL of the current tab
 *   is found.
 */
function getCurrentTabUrl(callback) {
  // Query filter to be passed to chrome.tabs.query - see
  // https://developer.chrome.com/extensions/tabs#method-query
  var queryInfo = {
    active: true,
    currentWindow: true
  };

  chrome.tabs.query(queryInfo, function(tabs) {
    // chrome.tabs.query invokes the callback with a list of tabs that match the
    // query. When the popup is opened, there is certainly a window and at least
    // one tab, so we can safely assume that |tabs| is a non-empty array.
    // A window can only have one active tab at a time, so the array consists of
    // exactly one tab.
    var tab = tabs[0];

    // A tab is a plain object that provides information about the tab.
    // See https://developer.chrome.com/extensions/tabs#type-Tab
    var url = tab.url;

    // tab.url is only available if the "activeTab" permission is declared.
    // If you want to see the URL of other tabs (e.g. after removing active:true
    // from |queryInfo|), then the "tabs" permission is required to see their
    // "url" properties.
    console.assert(typeof url == 'string', 'tab.url should be a string');

    callback(url);
  });
}

  function getUrlTitleAndImg(callback) {
      // Query filter to be passed to chrome.tabs.query - see
      // https://developer.chrome.com/extensions/tabs#method-query
      var queryInfo = {
          active: true,
          currentWindow: true
      };

      chrome.tabs.query(queryInfo, function(tabs) {
          // chrome.tabs.query invokes the callback with a list of tabs that match the
          // query. When the popup is opened, there is certainly a window and at least
          // one tab, so we can safely assume that |tabs| is a non-empty array.
          // A window can only have one active tab at a time, so the array consists of
          // exactly one tab.
          var tab = tabs[0];

          // A tab is a plain object that provides information about the tab.
          // See https://developer.chrome.com/extensions/tabs#type-Tab
          var url = tab.url;

          // tab.url is only available if the "activeTab" permission is declared.
          // If you want to see the URL of other tabs (e.g. after removing active:true
          // from |queryInfo|), then the "tabs" permission is required to see their
          // "url" properties.
          console.assert(typeof url == 'string', 'tab.url should be a string');
          var info = {
              url: url,
              title: tab.title,
              img: tab.favIconUrl
      };
          callback(info);
      });

}

/**
 * @param {string} searchTerm - Search term for Google Image search.
 * @param {function(string,number,number)} callback - Called when an image has
 *   been found. The callback gets the URL, width and height of the image.
 * @param {function(string)} errorCallback - Called when the image is not found.
 *   The callback gets a string that describes the failure reason.
 */
function getImageUrl(searchTerm, callback, errorCallback) {
  // Google image search - 100 searches per day.
  // https://developers.google.com/image-search/
  var searchUrl = 'https://ajax.googleapis.com/ajax/services/search/images' +
    '?v=1.0&q=' + encodeURIComponent(searchTerm);
  var x = new XMLHttpRequest();
  x.open('GET', searchUrl);
  // The Google image search API responds with JSON, so let Chrome parse it.
  x.responseType = 'json';
  x.onload = function() {
    // Parse and process the response from Google Image Search.
    var response = x.response;
    if (!response || !response.responseData || !response.responseData.results ||
        response.responseData.results.length === 0) {
      errorCallback('No response from Google Image search!');
      return;
    }
    var firstResult = response.responseData.results[0];
    // Take the thumbnail instead of the full image to get an approximately
    // consistent image size.
    var imageUrl = firstResult.tbUrl;
    var width = parseInt(firstResult.tbWidth);
    var height = parseInt(firstResult.tbHeight);
    console.assert(
        typeof imageUrl == 'string' && !isNaN(width) && !isNaN(height),
        'Unexpected respose from the Google Image Search API!');
    callback(imageUrl, width, height);
  };
  x.onerror = function() {
    errorCallback('Network error.');
  };
  x.send();
}

function renderText(text, elementId) {
    document.getElementById(elementId).textContent = text;
}

function renderHtml(html, elementId) {
    var element = document.getElementById(elementId);
    element.innerHTML = html;
}


document.addEventListener('DOMContentLoaded', function() {
    getUrlTitleAndImg
  (function (info) {
      var domain = info.url
      var startIndex = domain.indexOf("www.");
      var startLength = 4;
      if (startIndex == -1) {
          startIndex = domain.indexOf('//');
          startLength = 2;
      }
      domain = domain.substring(startIndex + startLength);
      var pointIndex = domain.indexOf(".");
      domain = domain.substring(0, pointIndex);

      var title = info.title.replace('- The New York Times', '');
      var newsImg = domain + '<img style =\'width:30px\' src=\'' + info.img + '\'/><span style=\'display:inline-block;width:10px\' ></span>';

      renderHtml(newsImg, 'Newsletter');
      renderText(title, 'ArticleHeadline');
      var url = "http://subtextserver20170724020220.azurewebsites.net/api/TopicOrientations?url=" + info.url;
	
	
      var xMLHttpRequest = new XMLHttpRequest();
      xMLHttpRequest.open('GET', url);
      xMLHttpRequest.responseType = 'json';
      xMLHttpRequest.onload = function () {
    var response = xMLHttpRequest.response;

	var html = "";
	var i;
          //var topicTemplate = '<li> @titleName, Favor: <span style=\'color '
	for (i = 0; i < response.length; i++) {
	    var orientation = response[i].Orientation;



	    var titleImgTemplate = '<div class="topic"><span>@topic</span><img src="@orientationNum.png" class="orImg"/></div>'

	    titleImgTemplate = titleImgTemplate.replace('@topic', response[i].Name);
	     
	    if (orientation < 30) {
	        orientation = 20;
	    }
	    else if (orientation < 50) {
	        orientation = 40;
	    }
	    else if (orientation < 70) {
	        orientation = 60;
	    }
	    else if (orientation < 90) {
	        orientation = 80;
	    }

	    titleImgTemplate = titleImgTemplate.replace('@orientationNum', orientation);

	    if (i === response.length - 1) {
	        titleImgTemplate = titleImgTemplate.replace('topic', 'topic topicLast');
	        
	    }
	    //var orientationNum = orientation < 40 ? 'red' : (orientation > 60 ? 'green' : 'gray');
	    //var OrientationColored = '<span style="color:' + color + '">' + orientation + '%</span>';
	    //text += '<li>' + response[i].Name + ', Orientation: ' + OrientationColored + '</li>' + "<br>";
	    html += titleImgTemplate;
	}
	//text = '<ol>' + text + '</ol>';
	renderHtml(html, 'topics');
	
  };
  xMLHttpRequest.onerror = function() {
   
  };
  xMLHttpRequest.send();
	
	//document.getElementById('topics').innerHTML = "<ol><li>topic 0</li></ol><br><ol><li>topic 1</li></ol><br><ol><li>topic 2</li></ol><br><ol><li>topic 3</li></ol><br><ol><li>topic 4</li></ol><br><ol><li>topic 5</li></ol><br><ol><li>topic 6</li></ol><br><ol><li>topic 7</li></ol><br><ol><li>topic 8</li></ol><br>";
	/* var imageResult = document.getElementById('image-result');
      // Explicitly set the width/height to minimize the number of reflows. For
      // a single image, this does not matter, but if you're going to embed
      // multiple external images in your page, then the absence of width/height
      // attributes causes the popup to resize multiple times.
      imageResult.width = 640;
      imageResult.height = 360;
      imageResult.src = 'http://www.metrolife234.com/wp-content/uploads/2017/02/donald-trump-2.jpg';
      imageResult.hidden = false; */
    //renderStatus('Performing Google Image search for ' + url);
/*
    getImageUrl(url, function(imageUrl, width, height) {

      renderStatus('Search term: ' + url + '\n' +
          'Google image search result: ' + imageUrl);
      var imageResult = document.getElementById('image-result');
      // Explicitly set the width/height to minimize the number of reflows. For
      // a single image, this does not matter, but if you're going to embed
      // multiple external images in your page, then the absence of width/height
      // attributes causes the popup to resize multiple times.
      imageResult.width = width;
      imageResult.height = height;
      imageResult.src = imageUrl;
      imageResult.hidden = false;

    }, function(errorMessage) {
      renderStatus('Cannot display image. ' + errorMessage);
    });*/
  }
  );
});
