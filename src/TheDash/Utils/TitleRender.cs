using Spectre.Console;
using Spectre.Console.Rendering;

public sealed class TitleRender : IRenderable
{
	string text;
	public TitleRender(string text){
		this.text = text;
	}
	public Measurement Measure(RenderOptions options, int maxWidth)
	{
		var width = maxWidth;
		return new Measurement(width, width);
	}

	public IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
	{
		Style style = new Style(Color.White, Color.Purple);
		var styling = new Style(style.Background);

		if(options.Capabilities.Unicode){
			string header = " " + text;
			yield return new Segment(header.PadRight(maxWidth), style);
			yield return Segment.LineBreak;
		}else{
			string header = " " + text;
			yield return new Segment(header.PadRight(maxWidth), style);
			yield return Segment.LineBreak;	
		}
	}
}
