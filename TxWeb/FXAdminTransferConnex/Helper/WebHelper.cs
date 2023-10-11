using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using FXAdminTransferConnex.Entities;

namespace FXAdminTransferConnexApp
{
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static MvcHtmlString GenerateMenu(this HtmlHelper helper)
        {
            List<UserAccessPermissions> parentMenuList = ProjectSession.UserAccessPermissions.Where(x => x.ParentMenuId == null).OrderBy(item => item.DispalyOrder).ToList();

            List<UserAccessPermissions> childMenuList = ProjectSession.UserAccessPermissions.Where(x => x.ParentMenuId != null).OrderBy(item => item.DispalyOrder).ToList();

            if (parentMenuList.Count > 0)
            {
                TagBuilder ul = new TagBuilder("ul");
                ul.MergeAttribute("class", "navigation navigation-main navigation-accordion restrictCall");
                ul.MergeAttribute("id", "sideBar");

                UrlHelper url = new UrlHelper(HttpContext.Current.Request.RequestContext);

                StringBuilder sb = new StringBuilder();

                foreach (UserAccessPermissions menu in parentMenuList)
                {
                    List<UserAccessPermissions> childList = childMenuList.Where(x => x.ParentMenuId == menu.MenuId).ToList();

                    TagBuilder liWithChild = new TagBuilder("li");

                    string anchorTag;

                    TagBuilder iTag = ImageTag(menu.ImagePath);

                    TagBuilder firstSpan = SpanTag();
                    firstSpan.InnerHtml = menu.Name;

                    if (childList.Any())
                    {
                        #region Menus With Child
                        TagBuilder aLink = AnchorLink(menu.Action, menu.Controller, url, true);
                        aLink.InnerHtml = string.Format(CultureInfo.InvariantCulture, "{0}{1}", Convert.ToString(iTag), Convert.ToString(firstSpan));
                        anchorTag = Convert.ToString(aLink);
                        #endregion
                    }
                    else
                    {
                        #region Menus Without Child
                        TagBuilder aLink = AnchorLink(menu.Action, menu.Controller, url, false);
                        aLink.InnerHtml = string.Format(CultureInfo.InvariantCulture, "{0}{1}", Convert.ToString(iTag), Convert.ToString(firstSpan));
                        anchorTag = Convert.ToString(aLink);
                        #endregion
                    }

                    #region Child Menus
                    string childmenuString = string.Empty;

                    if (childList.Any())
                    {
                        StringBuilder sbChild = new StringBuilder();

                        foreach (UserAccessPermissions childMenu in childList)
                        {


                            TagBuilder childLi = new TagBuilder("li");
                            childLi.MergeAttribute("class", "restrictCall");
                            TagBuilder aChildLink = AnchorLink(childMenu.Action, childMenu.Controller, url, false);

                            TagBuilder iChildTag = ImageTag(childMenu.ImagePath);

                            //Create menu icon span
                            TagBuilder childsecondSpan = SpanTag();
                            childsecondSpan.InnerHtml = childMenu.Name;

                            aChildLink.InnerHtml = string.Format(CultureInfo.InvariantCulture, "{0}{1}", Convert.ToString(iChildTag), Convert.ToString(childsecondSpan));

                            childLi.InnerHtml = string.Format(CultureInfo.InvariantCulture, "{0}", Convert.ToString(aChildLink));

                            sbChild.Append(Convert.ToString(childLi));
                        }

                        TagBuilder childUl = new TagBuilder("ul")
                        {
                            InnerHtml = sbChild.ToString()
                        };

                        childmenuString = Convert.ToString(childUl);
                    }
                    #endregion

                    liWithChild.InnerHtml = string.Format(CultureInfo.InvariantCulture, "{0}{1}", anchorTag, childmenuString);

                    sb.Append(Convert.ToString(liWithChild));
                }

                ul.InnerHtml = sb.ToString();
                return MvcHtmlString.Create(ul.ToString());
            }
            return MvcHtmlString.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="url"></param>
        /// <param name="isParent"></param>
        /// <returns></returns>
        private static TagBuilder AnchorLink(string actionName, string controllerName, UrlHelper url, bool isParent)
        {
            TagBuilder aLink = new TagBuilder("a");

            if (isParent)
            {
                aLink.MergeAttribute("class", "dropdown-toggle restrictCall");
                aLink.MergeAttribute("data-action", "click-trigger");
            }

            if (string.IsNullOrEmpty(actionName) || string.IsNullOrEmpty(controllerName))
            {
                aLink.MergeAttribute("href", "javascript:void(0);");
            }
            else
            {
                aLink.MergeAttribute("id", controllerName);
                aLink.MergeAttribute("href", url.Action(actionName, controllerName));
                aLink.MergeAttribute("class", "restrictCall");

            }
            return aLink;
        }

        private static TagBuilder SpanTag()
        {
            TagBuilder span = new TagBuilder("span");
            span.MergeAttribute("class", "restrictCall");
            return span;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        private static TagBuilder ImageTag(string className)
        {
            TagBuilder iTag = new TagBuilder("i");
            iTag.MergeAttribute("class", className + " restrictCall ");
            return iTag;
        }
    }

    public static class WebHelper
    {
        public static ReadOnlyDictionary<string, object> StatusColumnStyle { get; } =
      new ReadOnlyDictionary<string, object>(new Dictionary<string, object>
      {
            { "align", "center" },
            { "style", "text-align:center;vertical-align:middle !important;" },
            { "width", "60px" }
      });

        //public static readonly Dictionary<string, object> StatusColumnStyle = new Dictionary<string, object> { { "align", "center" }, { "style", "text-align:center;vertical-align:middle !important;" }, { "width", "60px" } };

        public static bool IsAuthorized(ActionExecutingContext filterContext)
        {
            List<UserAccessPermissions> userAccessPermissions = ProjectSession.UserAccessPermissions.Where(x =>
                String.Equals(x.Controller, filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, StringComparison.CurrentCultureIgnoreCase) && x.IsView.GetValueOrDefault()).ToList();

            return (userAccessPermissions.Count > 0);
        }

        public static bool CanAdd()
        {
            string controllerName = GetCurrentController();
            return ProjectSession.UserAccessPermissions.Any(item => item.IsAdd.GetValueOrDefault() && item.Controller != null && item.Controller.ToLower() == controllerName.ToLower());
        }

        public static bool CanEdit()
        {
            string controllerName = GetCurrentController();
            return ProjectSession.UserAccessPermissions.Any(item => item.IsEdit.GetValueOrDefault() && item.Controller != null && item.Controller.ToLower() == controllerName.ToLower());
        }

        public static bool CanDelete()
        {
            string controllerName = GetCurrentController();
            return ProjectSession.UserAccessPermissions.Any(item => item.IsDelete.GetValueOrDefault() && item.Controller != null && item.Controller.ToLower() == controllerName.ToLower());
        }

        public static string GetCurrentController()
        {
            var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;
            if (routeValues.ContainsKey("controller"))
                return (string)routeValues["controller"];
            return string.Empty;
        }
        public static bool CanView()
        {
            string controllerName = GetCurrentController();
            return ProjectSession.UserAccessPermissions.Any(item => item.IsView.GetValueOrDefault() && item.Controller != null && item.Controller.ToLower() == controllerName.ToLower());
        }
    }
}