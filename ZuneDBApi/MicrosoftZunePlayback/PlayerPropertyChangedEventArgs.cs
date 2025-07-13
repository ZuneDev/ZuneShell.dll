using System;

namespace MicrosoftZunePlayback;

public class PlayerPropertyChangedEventArgs : EventArgs
{
	private string _key;

	private object _value;

	public object Value => _value;

	public string Key => _key;

	public PlayerPropertyChangedEventArgs(string key, object value)
	{
		_key = key;
		_value = value;
	}
}
