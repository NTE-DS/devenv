/***************************************************************************************************
 * NasuTek Developer Studio
 * Copyright (C) 2005-2013 NasuTek Enterprises
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 ***************************************************************************************************/

using System;

namespace NasuTek.DevEnvironment.Resources
{
	public enum DockLocation : byte
	{
		Top		 = 1,
		Left	 = 2,
		Bottom	 = 3,
		Right	 = 4,
		Floating = 5
	}

	[Flags]
	public enum AllowedDockLocation : byte
	{
		Top		 = 0x01,
		Left	 = 0x02,
		Bottom	 = 0x04,
		Right	 = 0x08,
		Floating = 0x10
	}
}
