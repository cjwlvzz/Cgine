#pragma once
#include "ComponentsCommon.h"

namespace Cgine::transform
{
	DEFINE_TYPED_ID(transform_id);

	struct init_info
	{
		f32 position[3]{};
		//quaternion
		f32 rotation[4]{};
		f32 scale[3]{ 1.0f,1.0f,1.0f };
	};

	transform_id craete_transform(const init_info& info, Cgine::game_entity::entity_id entity_id);
	void remove_transform(transform_id id);
}