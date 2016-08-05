using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;

public class GlobusHttpHelper
{
    public CookieCollection gCookies;

    private HttpWebRequest gRequest;

    private HttpWebResponse gResponse;

    public string UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:18.0) Gecko/20100101 Firefox/18.0";

    private string proxyAddress = string.Empty;

    private int port = 80;

    private string proxyUsername = string.Empty;

    private string proxyPassword = string.Empty;

    public GlobusHttpHelper()
    {
    }

   

    public string getHtmlfromUrl(Uri url)
    {
        string str;
        string empty = string.Empty;
        try
        {
            this.setExpect100Continue();
            this.gRequest = (HttpWebRequest)WebRequest.Create(url);
            this.gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:18.0) Gecko/20100101 Firefox/18.0";
            
            this.gRequest.UserAgent = "Mozilla / 5.0(Windows NT 10.0; WOW64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 51.0.2704.103 Safari / 537.36";
            this.gRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            this.gRequest.Headers["Accept-Charset"] = "ISO-8859-1,utf-8;q=0.7,*;q=0.7";
            this.gRequest.Headers["Accept-Language"] = "en-us,en;q=0.8";
            this.gRequest.KeepAlive = true;
            this.gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            this.gRequest.CookieContainer = new CookieContainer();
            this.gRequest.Method = "GET";
            this.gRequest.AllowAutoRedirect = true;
           this.ChangeProxy(this.proxyAddress, this.port, this.proxyUsername, this.proxyPassword);
            if ((this.gCookies == null ? false : this.gCookies.Count > 0))
            {
                this.setExpect100Continue();
                try
                {
                    this.gRequest.CookieContainer.Add(this.gCookies);
                }
                catch
                {
                    foreach (Cookie gCooky in this.gCookies)
                    {
                        gCooky.Domain = url.Host;
                        this.gRequest.CookieContainer.Add(gCooky);
                    }
                }
            }
            this.setExpect100Continue();
            this.gResponse = (HttpWebResponse)this.gRequest.GetResponse();
            if (this.gResponse.StatusCode == HttpStatusCode.OK)
            {
                this.setExpect100Continue();
                this.gResponse.Cookies = this.gRequest.CookieContainer.GetCookies(this.gRequest.RequestUri);
                if (this.gResponse.Cookies.Count > 0)
                {
                    if (this.gCookies == null)
                    {
                        this.gCookies = this.gResponse.Cookies;
                    }
                    else
                    {
                        foreach (Cookie cooky in this.gResponse.Cookies)
                        {
                            bool flag = false;
                            foreach (Cookie value in this.gCookies)
                            {
                                if (value.Name != cooky.Name)
                                {
                                    continue;
                                }
                                value.Value = cooky.Value;
                                flag = true;
                                break;
                            }
                            if (flag)
                            {
                                continue;
                            }
                            this.gCookies.Add(cooky);
                        }
                    }
                }
                StreamReader streamReader = new StreamReader(this.gResponse.GetResponseStream());
                empty = streamReader.ReadToEnd();
                streamReader.Close();
                str = empty;
            }
            else
            {
                str = "Error";
            }
        }
        catch
        {
            return empty;
        }
        return str;
    }

    public string getHtmlfromUrlProxy(Uri url, string proxyAddress, int port, string proxyUsername, string proxyPassword)
    {
        
        string str;
        string empty = string.Empty;
        try
        {
            this.setExpect100Continue();
            this.gRequest = (HttpWebRequest)WebRequest.Create(url);
            this.gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:18.0) Gecko/20100101 Firefox/18.0";
            this.gRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            this.gRequest.Headers["Accept-Charset"] = "ISO-8859-1,utf-8;q=0.7,*;q=0.7";
            this.gRequest.Headers["Accept-Language"] = "en-us,en;q=0.5";
            this.gRequest.KeepAlive = true;
            this.gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            this.gRequest.Headers["X-IsAJAXForm"] = "1";
            this.gRequest.Headers["X-Requested-With"] = "XMLHttpRequest";
            this.proxyAddress = proxyAddress;
            this.port = port;
            this.proxyUsername = proxyUsername;
            this.proxyPassword = proxyPassword;
            this.gRequest.ProtocolVersion=HttpVersion.Version10;           
            
            this.ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);
            this.gRequest.CookieContainer = new CookieContainer();
            this.gRequest.Method = "GET";
            this.gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            if ((this.gCookies == null ? false : this.gCookies.Count > 0))
            {
                this.setExpect100Continue();
                this.gRequest.CookieContainer.Add(this.gCookies);
            }
            this.setExpect100Continue();
            this.gResponse = (HttpWebResponse)this.gRequest.GetResponse();
            if (this.gResponse.StatusCode == HttpStatusCode.OK)
            {
                this.setExpect100Continue();
                this.gResponse.Cookies = this.gRequest.CookieContainer.GetCookies(this.gRequest.RequestUri);
                if (this.gResponse.Cookies.Count > 0)
                {
                    if (this.gCookies == null)
                    {
                        this.gCookies = this.gResponse.Cookies;
                    }
                    else
                    {
                        foreach (Cookie cooky in this.gResponse.Cookies)
                        {
                            bool flag = false;
                            foreach (Cookie gCooky in this.gCookies)
                            {
                                if (gCooky.Name != cooky.Name)
                                {
                                    continue;
                                }
                                gCooky.Value = cooky.Value;
                                flag = true;
                                break;
                            }
                            if (flag)
                            {
                                continue;
                            }
                            this.gCookies.Add(cooky);
                        }
                    }
                }
                StreamReader streamReader = new StreamReader(this.gResponse.GetResponseStream());
                empty = streamReader.ReadToEnd();
                streamReader.Close();
                str = empty;
            }
            else
            {
                str = "Error";
            }
        }
        catch
        {
            return empty;
        }
        return str;
    }

 
    public string postFormData(Uri formActionUrl, string postData, string Refer = "")
    {
        string str;
        string empty = string.Empty;
        try
        {
            this.gRequest = (HttpWebRequest)WebRequest.Create(formActionUrl);
            this.gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:18.0) Gecko/20100101 Firefox/18.0";
            this.gRequest.CookieContainer = new CookieContainer();
            this.gRequest.Method = "POST";
            this.gRequest.Accept = " text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8, */*";
            this.gRequest.KeepAlive = true;
            this.gRequest.ContentType = "application/x-www-form-urlencoded";
            if (Refer != "")
                this.gRequest.Referer = Refer;
            this.ChangeProxy(this.proxyAddress, this.port, this.proxyUsername, this.proxyPassword);
            if ((this.gCookies != null && this.gCookies.Count > 0))
            {
                this.setExpect100Continue();
                this.gRequest.CookieContainer.Add(this.gCookies);
            }
            try
            {
                this.setExpect100Continue();
                string.Format(postData, new object[0]);
                byte[] bytes = Encoding.GetEncoding(1252).GetBytes(postData);
                this.gRequest.ContentLength = (long)((int)bytes.Length);
                Stream requestStream = this.gRequest.GetRequestStream();
                requestStream.Write(bytes, 0, (int)bytes.Length);
                requestStream.Close();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            try
            {
                this.gResponse = (HttpWebResponse)this.gRequest.GetResponse();
            }
            catch (Exception exception1)
            {
                Console.WriteLine(exception1);
            }
            if (this.gResponse.StatusCode == HttpStatusCode.OK)
            {
                this.setExpect100Continue();
                this.gResponse.Cookies = this.gRequest.CookieContainer.GetCookies(this.gRequest.RequestUri);
                if (this.gResponse.Cookies.Count > 0)
                {
                    if (this.gCookies == null)
                    {
                        this.gCookies = this.gResponse.Cookies;
                    }
                    else
                    {
                        foreach (Cookie cooky in this.gResponse.Cookies)
                        {
                            bool flag = false;
                            foreach (Cookie gCooky in this.gCookies)
                            {
                                if (gCooky.Name != cooky.Name)
                                {
                                    continue;
                                }
                                gCooky.Value = cooky.Value;
                                flag = true;
                                break;
                            }
                            if (flag)
                            {
                                continue;
                            }
                            this.gCookies.Add(cooky);
                        }
                    }
                }
                StreamReader streamReader = new StreamReader(this.gResponse.GetResponseStream());
                empty = streamReader.ReadToEnd();
                streamReader.Close();
                str = empty;
            }
            else
            {
                str = "Error in posting data";
            }
        }
        catch
        {
            return empty;
        }
        return str;
    }


    public void setExpect100Continue()
    {
        if (ServicePointManager.Expect100Continue)
        {
            ServicePointManager.Expect100Continue = false;
        }
    }

    public void setExpect100ContinueToTrue()
    {
        if (!ServicePointManager.Expect100Continue)
        {
            ServicePointManager.Expect100Continue = true;
        }
    }

}