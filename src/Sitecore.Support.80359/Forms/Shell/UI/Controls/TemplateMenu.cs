using Sitecore.Data.Items;
using Sitecore.Form.Core.Configuration;
using Sitecore.Web.UI.Sheer;
using Sitecore.WFFM.Abstractions;
using System;
using System.IO;
using System.Text;
using System.Web.UI;

namespace Sitecore.Support.Forms.Shell.UI.Controls
{
  public class TemplateMenu : Sitecore.Web.UI.HtmlControls.Control
  {
    public EventHandler Change;

    public TemplateMenu()
    {
    }

    public TemplateMenu(string template) : this()
    {
      this.TemplateID = template;
      base.Attributes["class"] = "scfEntry";
    }

    private void ChangeTemplateField(string value)
    {
      this.TemplateFieldID = value;
    }

    protected override void DoRender(HtmlTextWriter output)
    {
      output.Write("<div" + base.ControlAttributes + ">");
      StringBuilder builder = new StringBuilder();
      builder.AppendFormat("<span class=\"scfFieldName\">{0}:</span>", this.FieldName);
      builder.AppendFormat("<select class=\"scfAborder\" id='select_{0}'", this.ID);
      builder.AppendFormat(" onchange=\"scForm.postEvent(this,event,'{0}.ChangeTemplateField(&quot;' + this.value + '&quot;)')\" >", this.ID);
      builder.Append("<option class='scfNotDefined'");
      if ((this.TemplateFieldID == null) || (this.TemplateFieldID == Sitecore.Data.ID.Null.ToString()))
      {
        builder.Append(" selected='selected'");
      }
      builder.AppendFormat("value='{0}'>{1}</option>", Sitecore.Data.ID.Null, DependenciesManager.ResourceManager.Localize("NOD_DEFINED"));
      output.Write(builder.ToString());
      if (!string.IsNullOrEmpty(this.TemplateID))
      {
        this.TemplateContent(StaticSettings.ContextDatabase.GetTemplate(this.TemplateID), output);
      }
      output.Write("</select>");
      output.Write("</div>");
    }

    internal void Redraw()
    {
      StringWriter writer = new StringWriter(new StringBuilder());
      HtmlTextWriter output = new HtmlTextWriter(writer);
      this.DoRender(output);
      SheerResponse.SetOuterHtml(this.ID, output.InnerWriter.ToString());
    }

    private void RenderTemplatePart(TemplateItem template, HtmlTextWriter writer)
    {
      foreach (TemplateSectionItem item in template.GetSections())
      {
        writer.Write("<optgroup  class=\"scEditorHeaderNavigatorSection\" label=\"" + item.DisplayName + "\">");
        foreach (TemplateFieldItem item2 in item.GetFields())
        {
          string str = item2.ID.ToString();
          writer.Write("<option id=\"" + str + "\" value=\"" + str + "\"");
          writer.Write(" class=\"scEditorHeaderNavigatorField\" ");
          if (str == this.TemplateFieldID)
          {
            writer.Write(" selected=\"selected\"");
          }
          writer.Write(">" + item2.DisplayName + "</option>");
        }
        writer.Write("</optgroup>");
      }
    }

    private void RenderTemplates(TemplateItem template, HtmlTextWriter writer)
    {
      if (template != null)
      {
        foreach (TemplateItem item in template.BaseTemplates)
        {
          if ((item.ID != TemplateIDs.StandardTemplate) || (this.ShowStandardField == "1"))
          {
            this.RenderTemplatePart(item, writer);
            this.RenderTemplates(item, writer);
          }
        }
      }
    }

    private void TemplateContent(TemplateItem template, HtmlTextWriter writer)
    {
      if (template != null)
      {
        this.RenderTemplatePart(template, writer);
        this.RenderTemplates(template, writer);
      }
    }

    public string FieldID
    {
      get
      {
        return base.GetViewStateString("fieldid");
      }

      set
      {
        base.SetViewStateString("fieldid", value);
      }
    }

    public string FieldName
    {
      get
      {
        return base.GetViewStateString("field");
      }

      set
      {
        base.SetViewStateString("field", value);
      }
    }

    public string ShowStandardField
    {
      get
      {
        return base.GetViewStateString("StandardField");
      }

      set
      {
        base.SetViewStateString("StandardField", value);
      }
    }

    public string TemplateFieldID
    {
      get
      {
        return base.GetViewStateString("templatefieldid");
      }

      set
      {
        base.SetViewStateString("templatefieldid", value);
      }
    }

    public string TemplateFieldName
    {
      get
      {
        return base.GetViewStateString("templatefield");


      }
      set
      {
        base.SetViewStateString("templatefield", value);
      }
    }

    public string TemplateID
    {
      get
      {
        return base.GetViewStateString("templateID");
      }

      set
      {
        base.SetViewStateString("templateID", value);
      }
    }
  }
}
