using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Globalization;
using MvcApp.Models;

namespace MvcApp.Controllers
{
public class HomeController : Controller
{
    public DefaultModelBinder ModelBinder { get; private set; }
    public HomeController()
    {
        this.ModelBinder = new DefaultModelBinder(GetValueProvider());
    }

    private IValueProvider GetValueProvider()
    {
        NameValueCollection requestData = new NameValueCollection();
        requestData.Add("[0].Key", "Foo");
        requestData.Add("[0].Value.Name", "Foo");
        requestData.Add("[0].Value.PhoneNo", "123456789");
        requestData.Add("[0].Value.EmailAddress", "Foo@gmail.com");

        requestData.Add("[1].Key", "Bar");
        requestData.Add("[1].Value.Name", "Bar");
        requestData.Add("[1].Value.PhoneNo", "987654321");
        requestData.Add("[1].Value.EmailAddress", "Bar@gmail.com");

        return new NameValueCollectionValueProvider(requestData, CultureInfo.InvariantCulture);
    }

    private void InvokeAction(string actionName)
    {
        ControllerDescriptor controllerDescriptor = new ReflectedControllerDescriptor(typeof(HomeController));
        ReflectedActionDescriptor actionDescriptor = (ReflectedActionDescriptor)controllerDescriptor.FindAction(ControllerContext, actionName);
        actionDescriptor.MethodInfo.Invoke(this,this.ModelBinder.GetParameterValues(actionDescriptor).ToArray());
    }

    public void Index()
    {
        InvokeAction("Action");
    }

    public void Action(IDictionary<string, Contact> contacts)
    {
        foreach (string key  in contacts.Keys)
        {
            Response.Write(key + "<br/>");
            Contact contact = contacts[key];
            Response.Write(string.Format("&nbsp;&nbsp;&nbsp;&nbsp;{0}: {1}<br/>", "Name", contact.Name));
            Response.Write(string.Format("&nbsp;&nbsp;&nbsp;&nbsp;{0}: {1}<br/>", "PhoneNo", contact.PhoneNo));
            Response.Write(string.Format("&nbsp;&nbsp;&nbsp;&nbsp;{0}: {1}<br/><br/>", "EmailAddress", contact.EmailAddress));
        }
    }
}
}

